using System.Globalization;
using System.Net.Http.Headers;
using CarvedRock.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CarvedRock.WebApp.Pages;

public partial class ListingModel : PageModel
{
    private readonly HttpClient _apiClient;
    private readonly ILogger<ListingModel> _logger;
    private readonly HttpContext? _httpCtx;

    public ListingModel(HttpClient apiClient, ILogger<ListingModel> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiClient.BaseAddress = new Uri("https://localhost:7213");
        _httpCtx = httpContextAccessor.HttpContext;
    }

    public List<ProductModel>? Products { get; set; }
    public string CategoryName { get; set; } = "";

    public async Task OnGetAsync()
    {
        _logger.LogInformation("Making API call to get products...");
        var cat = Request.Query["cat"].ToString();
        if (string.IsNullOrEmpty(cat))
        {
            throw new Exception("failed");
        }

        if (_httpCtx != null)
        {
            var accessToken = await _httpCtx.GetTokenAsync("access_token");
            _apiClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            // for a better way to include and manage access tokens for API calls:
            // https://identitymodel.readthedocs.io/en/latest/aspnetcore/web.html
        }

        var response = await _apiClient.GetAsync($"Product?category={cat}");
        if (response.IsSuccessStatusCode)
        {
            var jsonContent = await response.Content.ReadAsStringAsync();
            Products = JsonConvert.DeserializeObject<List<ProductModel>>(jsonContent); // Newtonsoft.Json

            if (Products != null && Products.Any())
            {
                CategoryName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Products.First().Category);
            }

            return;
        }
        // API call was not successful
        var fullPath = $"{_apiClient.BaseAddress}Product?category={cat}";

        var details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ??
                      new ProblemDetails();
        var traceId = details.Extensions["traceId"]?.ToString();

        _logger.LogWarning(
            "API failure: {fullPath} Response: {apiResponse}, Trace: {trace}",
            fullPath, (int)response.StatusCode, traceId);

        throw new Exception("API call failed!");
    }
}

