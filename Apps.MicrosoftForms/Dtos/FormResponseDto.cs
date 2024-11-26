namespace Apps.MicrosoftForms.Dtos;
public class FormResponseDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime SubmitDate { get; set; }
    public string Responder { get; set; }
    public string ResponderName { get; set; }
    public string Answers { get; set; }
    public DateTime ReleaseDate { get; set; }
}

