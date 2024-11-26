using Newtonsoft.Json;

namespace Apps.MicrosoftForms.Dtos;
public class BaseResponseDto<T>
{
    [JsonProperty("@odata.context")]
    public string OdataContext { get; set; }

    public List<T> Value { get; set; }
}

