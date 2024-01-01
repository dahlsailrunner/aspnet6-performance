using IdentityModel.Client;

namespace CarvedRock.LoadTest;
internal static class ClientCredentialsFactory 
{
    public static async Task<HttpClient> CreateClient(string name, string baseUrl)
    {
        var tokenClient = new HttpClient();
        var tokenResponse = await tokenClient.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = "https://demo.duendesoftware.com/connect/token",

                ClientId = "m2m",
                ClientSecret = "secret",
                Scope = "api"
            });

        var testClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        testClient.SetBearerToken(tokenResponse.AccessToken!);
        return testClient;
    }    
}
