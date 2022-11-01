using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarvedRock.WebApp.Pages
{
    public class CurrentPromotionModel : PageModel
    {
        public string RemoteContent { get; set; }

        public async Task OnGetAsync()
        {
            RemoteContent = await GetRemoteContentAsync();
        }

        public Task<string> GetRemoteContentAsync()
        {
            return Task.FromResult("Some content from a remote API or DB call.");
        }
    }
}
