using Blackbird.Applications.Sdk.Common;

namespace Apps.MicrosoftForms.Models.Request;
public class DownloadResponsesFileRequest
{
    [Display("Timezone offset")]
    public int? TimezoneOffset { get; set; }

    [Display("Min response ID")]
    public int? MinResponseId { get; set; }

    [Display("Max response ID")]
    public int? MaxResponseId { get; set; }
}

