using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.MicrosoftForms.Invocables;
using RestSharp;
using Apps.MicrosoftForms.Dtos;
using Apps.MicrosoftForms.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Apps.MicrosoftForms.Models.Request;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Net.Http.Headers;

namespace Apps.MicrosoftForms.Actions;

[ActionList]
public class FormActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : MicrosoftFormsInvocable(invocationContext)
{
    private readonly IFileManagementClient _fileManagementClient = fileManagementClient;


    [Action("List my forms", Description = "List my forms")]
    public async Task<ListFormsResponse> ListForms()
    {
        var request = new RestRequest("api/forms", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormInfoDto>>(request);
        return new() { Forms = response.Value };
    }

    [Action("Get form info", Description = "Get form info")]
    public async Task<FormInfoDto> GetForm([ActionParameter] GetFormRequest getFormRequest)
    {
        var request = new RestRequest($"api/forms('{getFormRequest.FormId}')", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<FormInfoDto>(request);
        return response;
    }

    [Action("List questions", Description = "List form questions")]
    public async Task<ListFormQuestionsResponse> ListFormQuestions([ActionParameter] GetFormRequest getFormRequest)
    {
        var request = new RestRequest($"api/forms('{getFormRequest.FormId}')/questions", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormQuestionDto>>(request);
        return new() { Questions = response.Value };
    }

    [Action("List responses", Description = "List form responses")]
    public async Task<ListFormResponsesResponse> ListFormResponses([ActionParameter] GetFormRequest getFormRequest)
    {
        var request = new RestRequest($"api/forms('{getFormRequest.FormId}')/responses", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormResponseDto>>(request);
        return new() { Responses = response.Value };
    }

    [Action("Get answer from response", Description = "Get question answer from response")]
    public async Task<string> GetAnswerFromResponse(
        [ActionParameter] GetFormRequest getFormRequest,
        [ActionParameter] GetFormQuestionRequest getFormQuestionRequest,
        [ActionParameter] GetFormResponseRequest getFormResponseRequest)
    {
        var responseRequest = new RestRequest($"api/forms('{getFormRequest.FormId}')/responses", Method.Get);
        var responseResponse = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormResponseDto>>(responseRequest);

        var selectedResponse = responseResponse.Value.FirstOrDefault(x => x.Id.ToString() == getFormResponseRequest.ResponseId);
        if (selectedResponse == null)
            throw new PluginMisconfigurationException($"Response with id {getFormResponseRequest.ResponseId} not found");

        foreach(JObject item in JArray.Parse(selectedResponse.Answers))
        {
            if (item.GetValue("answer1")?.Type == JTokenType.Object && item["answer1"]?.SelectToken("displayText") != null)
                return item?["answer1"]?["displayText"]?.ToString() ?? "";

            if (item.GetValue("questionId")?.ToObject<string>() == getFormQuestionRequest.QuestionId)
                return item.GetValue("answer1")?.ToObject<string>() ?? "";
        }
        throw new PluginMisconfigurationException($"Question with id {getFormQuestionRequest.QuestionId} not found");
    }

    [Action("Download responses as excel file", Description = "Download responses as excel file (.xlsx)")]
    public async Task<DownloadResponsesFileResponse> DownloadResponsesFile(
        [ActionParameter] GetFormRequest getFormRequest,
        [ActionParameter] DownloadResponsesFileRequest downloadResponsesFileRequest)
    {
        var request = new RestRequest($"DownloadExcelFile.ashx", Method.Get);
        request.AddQueryParameter("formid", getFormRequest.FormId);
        request.AddQueryParameter("timezoneOffset", downloadResponsesFileRequest.TimezoneOffset ?? 180);
        request.AddQueryParameter("minResponseId", downloadResponsesFileRequest.MinResponseId ?? 1);
        request.AddQueryParameter("maxResponseId", downloadResponsesFileRequest.MaxResponseId ?? 1000);
        var response = await Client.ExecuteWithErrorHandling(request);

        using var stream = new MemoryStream(response.RawBytes);
        var contentDisposition = ContentDispositionHeaderValue.Parse(response.ContentHeaders.First(x => x.Name == "Content-Disposition").Value.ToString());
        var responsesFile = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Application.Octet, contentDisposition.FileNameStar);
        return new() { ResponsesFile = responsesFile };
    }

    //[Action("Debug", Description = "Debug")]
    //public async Task<string> Debug()
    //{
    //    var token = InvocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
    //    return token;
    //}
}