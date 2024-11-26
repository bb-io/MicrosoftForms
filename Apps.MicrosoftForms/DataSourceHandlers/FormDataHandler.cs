using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.MicrosoftForms.Dtos;
using Apps.MicrosoftForms.Invocables;
using RestSharp;

namespace Apps.MicrosoftForms.DataSourceHandlers;
public class FormDataHandler : MicrosoftFormsInvocable, IAsyncDataSourceHandler
{
    public FormDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new RestRequest("api/forms", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormInfoDto>>(request);
        return response.Value.Where(x => context.SearchString is null || x.Title.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)).ToDictionary(k => k.Id.ToString(), v => v.Title);
    }
}

