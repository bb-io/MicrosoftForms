using Apps.MicrosoftForms.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftForms.Models.Request;
public class GetFormRequest
{
    [Display("Form ID")]
    [DataSource(typeof(FormDataHandler))]
    public string FormId { get; set; }
}

