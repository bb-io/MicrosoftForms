using Apps.MicrosoftForms.Dtos;
using Apps.MicrosoftForms.Invocables;
using Apps.MicrosoftForms.Models.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.MicrosoftForms.DataSourceHandlers;
public class FormResponseDataHandler : MicrosoftFormsInvocable, IAsyncDataSourceHandler
{
    private GetFormRequest FormRequest { get; set; }

    public FormResponseDataHandler(InvocationContext invocationContext, [ActionParameter] GetFormRequest formRequest) : base(invocationContext)
    {
        FormRequest = formRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (FormRequest == null)
            throw new ArgumentException("Please specify Form ID first");

        var request = new RestRequest($"api/forms('{FormRequest.FormId}')/responses", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormResponseDto>>(request);
        return response.Value
            .Where(x => context.SearchString is null || x.Id.ToString().Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Id)
            .ToDictionary(k => k.Id.ToString(), v => v.Id.ToString());
    }
}

