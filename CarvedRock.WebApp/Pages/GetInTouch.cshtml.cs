using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarvedRock.WebApp.Pages;

[AllowAnonymous]
public class GetInTouchModel : PageModel
{
    public void OnGet()
    {
    }

    public void OnPost()
    {
        var content = Request.Form["content"];
        var emailAddress = Request.Form["emailaddress"];
        
        // store stuff and get back to user somehow
    }
}

