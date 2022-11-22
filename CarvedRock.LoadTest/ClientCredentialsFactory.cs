using IdentityModel.Client;
using NBomber.Contracts;
using NBomber.CSharp;

namespace CarvedRock.LoadTest;
internal static class ClientCredentialsFactory
{
    public static IClientFactory<HttpClient> GetClientFactory(string name, string baseUrl)
    {
        return ClientFactory.Create(
            name: name,
            clientCount: 1,
            initClient: async (_, _) =>
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
                testClient.SetBearerToken(tokenResponse.AccessToken);
                return testClient;
            });
    }
}
