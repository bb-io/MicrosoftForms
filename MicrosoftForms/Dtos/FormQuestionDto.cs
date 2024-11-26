namespace Apps.MicrosoftForms.Dtos;
public class FormQuestionDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public double Order { get; set; }
    public string Type { get; set; }
    public bool Required { get; set; }
    public string QuestionInfo { get; set; }
    public bool IsQuiz { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string Status { get; set; }
    public string Subtitle { get; set; }
    public bool TitleHasPhishingKeywords { get; set; }
    public bool SubtitleHasPhishingKeywords { get; set; }
    public bool IsFromSuggestion { get; set; }
    public string InsightsInfo { get; set; }
    public string GroupId { get; set; }
    public string FormsProRTQuestionTitle { get; set; }
    public string FormsProRTSubtitle { get; set; }
    public string TrackingId { get; set; }
}

