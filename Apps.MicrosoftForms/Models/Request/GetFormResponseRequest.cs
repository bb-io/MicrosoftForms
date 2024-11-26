using Apps.MicrosoftForms.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftForms.Models.Request;
public class GetFormResponseRequest
{
    [Display("Response ID")]
    [DataSource(typeof(FormResponseDataHandler))]
    public string ResponseId { get; set; }
}

