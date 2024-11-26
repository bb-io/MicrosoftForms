using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MicrosoftForms.Models.Response;
public class DownloadResponsesFileResponse
{
    [Display("Responses file")]
    public FileReference ResponsesFile { get; set; }
}

