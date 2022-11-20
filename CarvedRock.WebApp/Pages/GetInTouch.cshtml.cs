using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarvedRock.WebApp.Pages;

[AllowAnonymous]
public class GetInTouchModel : PageModel
{
    [BindProperty]
    public string Content { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPostAsync()
    {
        var bestContent = Content;
        var form = await Request.ReadFormAsync();

        var betterContent = form["content"];
        var betterEmail = form["emailaddress"];

        var content = Request.Form["content"];
        var emailAddress = Request.Form["emailaddress"];
        
        // store stuff and get back to user somehow
    }
}

