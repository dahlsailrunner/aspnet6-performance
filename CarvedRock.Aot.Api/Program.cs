using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text.Json.Serialization;
using CarvedRock.Aot.Api;
using Microsoft.IdentityModel.Tokens;
using CarvedRock.Domain;
using CarvedRock.Data;
using CarvedRock.Core;
using CarvedRock.Data.CompiledModels;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) => {
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name ?? "API")
        .Enrich.WithExceptionDetails()
        .Enrich.FromLogContext()
        .Enrich.With<ActivityEnricher>()
        .WriteTo.Seq("http://localhost:5341")
        .WriteTo.Console()
        .WriteTo.Debug();
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
builder.Services.AddProblemDetails();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.Audience = "api";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "email"
        };
        options.SaveToken = true;
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<IApiCaller, ApiCaller>(client =>
{
    client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CarvedRock";
});

builder.Services.AddScoped<IProductLogic, ProductLogic>();
builder.Services.AddScoped<IExtraLogic, ExtraLogic>();

builder.Services.AddDbContext<LocalContext>(options => options.UseModel(LocalContextModel.Instance));
builder.Services.AddScoped<ICarvedRockRepository, CarvedRockRepository>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseResponseCompression();
app.UseResponseCaching();
app.UseAuthentication();
app.UseMiddleware<UserScopeMiddleware>();
app.UseAuthorization();

ProductEndpoints.Map(app);
TodoEndpoints.Map(app);

app.Run();

[JsonSerializable(typeof(IEnumerable<ProductModel>))]
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
