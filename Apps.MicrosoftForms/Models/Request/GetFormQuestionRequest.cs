using Apps.MicrosoftForms.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftForms.Models.Request;
public class GetFormQuestionRequest
{
    [Display("Question ID")]
    [DataSource(typeof(FormQuestionDataHandler))]
    public string QuestionId { get; set; }
}

