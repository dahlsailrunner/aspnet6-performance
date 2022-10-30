using CarvedRock.Core;
using CarvedRock.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarvedRock.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class SyncProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductLogic _productLogic;   

    
    public SyncProductController(ILogger<ProductController> logger, IProductLogic productLogic)
    {
        _logger = logger;
        _productLogic = productLogic;
    }

    [HttpGet]
    public IEnumerable<ProductModel> Get(string category = "all")
    {
        using (_logger.BeginScope("ScopeCat: {ScopeCat}", category))
        {     
            _logger.LogInformation( "Getting products in API.");
            return _productLogic.GetProductsForCategory(category);
        }
    }
}