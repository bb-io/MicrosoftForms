using Apps.MicrosoftForms.Dtos;
using Apps.MicrosoftForms.Invocables;
using Apps.MicrosoftForms.Models.Request;
using Azure;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.MicrosoftForms.DataSourceHandlers;
public class FormQuestionDataHandler : MicrosoftFormsInvocable, IAsyncDataSourceHandler
{
    private GetFormRequest FormRequest { get; set; }

    private readonly List<string> IgnoreColumnTypes = new List<string>() { "Question.ColumnGroup", "Question.MatrixChoiceGroup" };
    public FormQuestionDataHandler(InvocationContext invocationContext, [ActionParameter] GetFormRequest formRequest) : base(invocationContext)
    {
        FormRequest = formRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (FormRequest == null)
            throw new ArgumentException("Please specify Form ID first");

        var request = new RestRequest($"api/forms('{FormRequest.FormId}')/questions", Method.Get);
        var response = await Client.ExecuteWithErrorHandling<BaseResponseDto<FormQuestionDto>>(request);
        return response.Value
            .Where(x => !IgnoreColumnTypes.Contains(x.Type))
            .Where(x => context.SearchString is null || x.Title.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Order)
            .ToDictionary(k => k.Id.ToString(), v => CreateDisplayName(v, response.Value));
    }

    private string CreateDisplayName(FormQuestionDto formQuestion, List<FormQuestionDto> formQuestions)
    {
        if (string.IsNullOrEmpty(formQuestion.GroupId))
            return formQuestion.Title;


        var groupElement = formQuestions.FirstOrDefault(x => x.Id == formQuestion.GroupId);
        if(groupElement == null)
            return formQuestion.Title;

        return $"{formQuestion.Title} ({groupElement.Title})";
    }
}

