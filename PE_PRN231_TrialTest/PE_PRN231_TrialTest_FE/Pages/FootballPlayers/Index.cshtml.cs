using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public IndexModel(
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public IList<FootballPlayerResponse> FootballPlayers { get; set; } = new List<FootballPlayerResponse>();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");
            if (role == "1" || role == "2")
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return RedirectToPage("/Login");
                }
                // Set the authorization header with Bearer token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var apiUrl = $"http://localhost:5213/api/premier-leauge/players";
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<IList<FootballPlayerResponse>>(apiUrl);
                    if (response != null)
                        FootballPlayers = response;
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Unable to retrieve players! {ex.Message}");
                }
                return Page();
            }
            return RedirectToPage("/Login");
        }
    }

    public class FootballPlayerResponse
    {
        public string FootballPlayerId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Achievements { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string PlayerExperiences { get; set; } = null!;

        public string Nomination { get; set; } = null!;

        public string? FootballClubId { get; set; }

        public string ClubName { get; set; } = null!;
    }
}
