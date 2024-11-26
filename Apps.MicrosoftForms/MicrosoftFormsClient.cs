using Apps.MicrosoftForms.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Apps.MicrosoftForms;

public class MicrosoftFormsClient : BlackBirdRestClient
{

    public MicrosoftFormsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(
            new RestClientOptions { ThrowOnAnyError = true, BaseUrl = GetUri(authenticationCredentialsProviders) }
        )
    {
        var token = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
        this.AddDefaultHeader("Authorization", $"Bearer {token}");
    }

    private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider)
    {
        return new($"https://forms.office.com/formapi");
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        try
        {
            var errorDto = JsonConvert.DeserializeObject<ErrorDto>(response.Content);
            return new PluginMisconfigurationException(errorDto.Error.Message);
        }
        catch (Exception ex)
        {
            return new PluginApplicationException(response.Content);
        }
    }
}