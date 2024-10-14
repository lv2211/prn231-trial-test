using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public FootballPlayerResponse FootballPlayer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");
            if (role == "1")
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
                var apiUrl = $"http://localhost:5213/api/premier-leauge/players/{id}";
                // Set the authorization header with Bearer token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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

        public async Task<IActionResult> OnPostAsync(string id)
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
            // Set the authorization header with Bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var apiGetUrl = $"http://localhost:5213/api/premier-leauge/players/{id}";
            var apiDeleteUrl = $"http://localhost:5213/api/premier-leauge/player/{id}";

            var footballplayer = await _httpClient.GetFromJsonAsync<FootballPlayerResponse>(apiGetUrl);
            if (footballplayer != null)
            {
                FootballPlayer = footballplayer;
                var result = await _httpClient.DeleteAsync(apiDeleteUrl);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else ModelState.AddModelError(string.Empty, "Unable to delete player!");
            }
            return Page();
        }
    }
}
