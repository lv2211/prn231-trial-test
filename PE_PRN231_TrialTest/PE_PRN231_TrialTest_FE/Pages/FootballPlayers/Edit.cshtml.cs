using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public FootballPlayerResponse FootballPlayer { get; set; } = default!;
        
        public UpdateFootballPlayerRequest? UpdateFootballPlayerRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");
            if (role == "1")
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (id == null)
                {
                    return NotFound();
                }
                var apiGetPlayer = $"http://localhost:5213/api/premier-leauge/players/{id}";
                var apiGetClub = "http://localhost:5213/api/premier-leauge/clubs";
                
                // Set the authorization header with Bearer token
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var footballClubs = await _httpClient.GetFromJsonAsync<IEnumerable<FootballClub>>(apiGetClub);
                var footballplayer = await _httpClient.GetFromJsonAsync<FootballPlayerResponse>(apiGetPlayer);
                if (footballplayer == null)
                {
                    return NotFound();
                }
                FootballPlayer = footballplayer;
                ViewData["FootballClubId"] = new SelectList(footballClubs, "FootballClubId", "ClubName");
                return Page();
            }
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Login");
            }
            UpdateFootballPlayerRequest updateRequest = new UpdateFootballPlayerRequest
            {
                FootballPlayerId = FootballPlayer.FootballPlayerId,
                FootballClubId = FootballPlayer.FootballClubId,
                Achievements = FootballPlayer.Achievements,
                Birthday = FootballPlayer.Birthday,
                FullName = FootballPlayer.FullName,
                Nomination = FootballPlayer.Nomination,
                PlayerExperiences = FootballPlayer.PlayerExperiences
            };
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            // Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var apiUpdate = $"http://localhost:5213/api/premier-leauge/player/{FootballPlayer.FootballPlayerId}";
            // Serialize the request to JSON
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            try
            {
                var result = await _httpClient.PutAsync(apiUpdate, content);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the football player. {ex.Message}");
            }
            return Page();
        }
    }

    public class UpdateFootballPlayerRequest
    {
        [Required(ErrorMessage = "Field is required!")]
        public string FootballPlayerId { get; set; } = null!;

        [Required(ErrorMessage = "Field is required!")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9@#\s]*$", ErrorMessage = "Invalid Fullname!")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Field is required!")]
        [StringLength(100, MinimumLength = 9, ErrorMessage = "Achievement must be in 9-100 characters!")]
        public string Achievements { get; set; } = null!;

        [Required(ErrorMessage = "Field is required!")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string PlayerExperiences { get; set; } = null!;

        [Required(ErrorMessage = "Field is required!")]
        [StringLength(100, MinimumLength = 9, ErrorMessage = "Nomination must be in 9-100 characters!")]
        public string Nomination { get; set; } = null!;

        [Required(ErrorMessage = "Field is required!")]
        public string? FootballClubId { get; set; }
    }
}
