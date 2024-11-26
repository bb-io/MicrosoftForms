using Newtonsoft.Json;

namespace Apps.MicrosoftForms.Dtos;
public class FormInfoDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Version { get; set; }
    public string OwnerId { get; set; }
    public string OwnerTenantId { get; set; }
    public string Settings { get; set; }
    public int SoftDeleted { get; set; }
    public string ThankYouMessage { get; set; }
    public int Flags { get; set; }
    public string Type { get; set; }
    public string DefaultLanguage { get; set; }
    public string FormsProRTTitle { get; set; }
    public string FormsProRTDescription { get; set; }
    public int FillOutTimeLimit { get; set; }
    public int TenantSwitches { get; set; }
    public string PrivacyUrl { get; set; }
    public string Description { get; set; }
    public int OnlineSafetyLevel { get; set; }
    public int ReputationTier { get; set; }
    public string TableId { get; set; }
    public string OtherInfo { get; set; }
    public string Status { get; set; }
    public string Category { get; set; }
    public int FillOutRemainingTime { get; set; }
    public DateTime TimedFormStartTime { get; set; }
    public string DistributionInfo { get; set; }
    public string CreatedBy { get; set; }
    public bool XlFileUnSynced { get; set; }

    [JsonProperty("rowCount@odata.type")]
    public string RowCountOdataType { get; set; }
    public int RowCount { get; set; }
    public string TrackingId { get; set; }
    public string ThemeV2 { get; set; }
}

