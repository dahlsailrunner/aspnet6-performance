using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarvedRock.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CarvedRock.Domain;

public class ApiCaller : IApiCaller
{
    private HttpContext _httpContext;

    public ApiCaller(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<List<LocalClaim>?> CallExternalApiAsync()
    {
        using (var httpCli = new HttpClient { BaseAddress = new Uri("https://demo.duendesoftware.com/api/") })
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            httpCli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpCli.GetAsync("test");
            response.EnsureSuccessStatusCode();

            var claims = await response.Content.ReadFromJsonAsync<List<LocalClaim>>();
            return claims;
        }
    }
}
