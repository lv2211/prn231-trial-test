using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public FootballPlayerResponse FootballPlayer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");
            if (role == "1" || role == "2")
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return RedirectToPage("/Login");
                }
                if (id == null)
                {
                    return NotFound();
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var apiUrl = $"http://localhost:5213/api/premier-leauge/players/{id}";
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<FootballPlayerResponse>(apiUrl);
                    if (response != null)
                        FootballPlayer = response;
                    else return NotFound();
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Unable to retrieve player! {ex.Message}");
                }
                return Page();
            }
            return RedirectToPage("/Login");
        }
    }
}
