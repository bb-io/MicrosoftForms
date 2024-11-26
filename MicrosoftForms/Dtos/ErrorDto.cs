namespace Apps.MicrosoftForms.Dtos;
public class ErrorDto
{
    public ErrorValue Error { get; set; }
}

public class ErrorValue
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public object CustomizedMessage { get; set; }
}
