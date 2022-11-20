using System.Net.Http.Headers;
using System.Net.Http.Json;
using CarvedRock.Core;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CarvedRock.Domain;

public class ApiCaller : IApiCaller
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiCaller(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<LocalClaim>?> CallExternalApiAsync()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new Exception("Can't get access token.");
        }

        var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        _httpClient.SetBearerToken(token);

        var response = await _httpClient.GetAsync("test");
        response.EnsureSuccessStatusCode();

        var claims = await response.Content.ReadFromJsonAsync<List<LocalClaim>>();
        return claims;
    }
}
