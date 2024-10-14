using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PE.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace PE_PRN231_TrialTest_FE.Pages.FootballPlayers
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet()
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("Role");
            if (role == "1")
            {
                var apiGetClub = "http://localhost:5213/api/premier-leauge/clubs";

                var footballClubs = await _httpClient.GetFromJsonAsync<IEnumerable<FootballClub>>(apiGetClub);
                ViewData["FootballClubId"] = new SelectList(footballClubs, "FootballClubId", "ClubName");
                return Page();
            }
            return RedirectToPage("/Login");
        }

        [BindProperty]
        public CreateFootballPlayerRequest FootballPlayer { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToPage("/Login");
            }
            if (!ModelState.IsValid)
            {
                return await OnGet();
            }
            // Authorization header with Bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var apiCreate = "http://localhost:5213/api/premier-leauge/player";
            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiCreate, FootballPlayer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create player! {ex.Message}");
                //return await OnGet();
            }
            return Page();
        }
    }

    /// <summary>
    /// Request model
    /// </summary>
    public class CreateFootballPlayerRequest
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

    /// <summary>
    /// Club
    /// </summary>
    public class FootballClub
    {
        public string FootballClubId { get; set; } = null!;

        public string ClubName { get; set; } = null!;

        public string ClubShortDescription { get; set; } = null!;

        public string SoccerPracticeField { get; set; } = null!;

        public string Mascos { get; set; } = null!;

        public virtual ICollection<FootballPlayer> FootballPlayers { get; set; } = new List<FootballPlayer>();
    }
}
