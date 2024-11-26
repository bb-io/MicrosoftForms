using Apps.MicrosoftForms.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.MicrosoftForms.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        CancellationToken cancellationToken)
    {
        var client = new MicrosoftFormsClient(authenticationCredentialsProviders);

        try
        {
            var request = new RestRequest("api/forms", Method.Get);
            var response = await new MicrosoftFormsClient(authenticationCredentialsProviders).ExecuteWithErrorHandling<BaseResponseDto<FormInfoDto>>(request);
            return new ConnectionValidationResponse
            {
                IsValid = true,
                Message = "Success"
            };
        }
        catch (Exception)
        {
            return new ConnectionValidationResponse
            {
                IsValid = false,
                Message = "Ping failed"
            };
        }
    }
}