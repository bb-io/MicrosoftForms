using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Apps.MicrosoftForms.Webhooks.Payload;
using Apps.MicrosoftForms.Dtos;
using Apps.MicrosoftForms.Models.Request;
using RestSharp;
using Apps.MicrosoftForms.Invocables;
using Apps.MicrosoftForms.Models.Response;

namespace Apps.MicrosoftForms.Webhooks;

[PollingEventList]
public class PollingList(InvocationContext invocationContext) : MicrosoftFormsInvocable(invocationContext)
{
    [PollingEvent("On responses received", "This webhook is triggered when new responses are received.")]
    public async Task<PollingEventResponse<LastResponseMemory, ListFormResponsesResponse>> OnResponsesReceived(
        PollingEventRequest<LastResponseMemory> request,
        [PollingEventParameter] GetFormRequest formRequest)
    {
        if (request.Memory == null)
        {
            GetNewReceivedResponses(null, formRequest, out var newResponseTime);
            return new()
            {
                FlyBird = false,
                Memory = new() { LastResponseTime = newResponseTime }
            };
        }

        var receivedResponses = GetNewReceivedResponses(request.Memory.LastResponseTime, formRequest, out var newLastResponseTime);

        if (receivedResponses.Count() == 0)
        {
            return new()
            {
                FlyBird = false,
                Memory = new() { LastResponseTime = newLastResponseTime }
            };
        }

        return new()
        {
            FlyBird = true,
            Memory = new() { LastResponseTime = newLastResponseTime },
            Result = new() { Responses = receivedResponses.ToList() }
        };
    }

    private IEnumerable<FormResponseDto> GetNewReceivedResponses(DateTime? previousSubmitDateTime, GetFormRequest formRequest, out DateTime newLastSubmitTime)
    {
        var request = new RestRequest($"api/forms('{formRequest.FormId}')/responses", Method.Get);
        var response = Client.ExecuteWithErrorHandling<BaseResponseDto<FormResponseDto>>(request).Result;
        if(previousSubmitDateTime == null)
        {
            if(response == null || response.Value == null || !response.Value.Any())
                newLastSubmitTime = DateTime.MinValue;
            else
                newLastSubmitTime = response.Value.OrderBy(x => x.SubmitDate).Last().SubmitDate;
            return Enumerable.Empty<FormResponseDto>();
        }
        var newResponses = response.Value.Where(x => x.SubmitDate > previousSubmitDateTime).ToList();
        if (newResponses.Any())
            newLastSubmitTime = newResponses.OrderBy(x => x.SubmitDate).Last().SubmitDate;
        else
            newLastSubmitTime = previousSubmitDateTime.Value;
        return newResponses;
    }
}

