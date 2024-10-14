using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PE_PRN231_TrialTest_FE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var signinApi = "http://localhost:5213/api/premier-league/account/sign-in";
            var response = await _httpClient.PostAsJsonAsync(signinApi, LoginRequest);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<AccountResponse>();
                if (loginResponse is not null)
                {
                    if (loginResponse.Role == "3" || loginResponse.Role == "4")
                    {
                        ModelState.AddModelError(string.Empty, "You are not authorized to access this page.");
                        return Page();
                    }
                    _httpContextAccessor.HttpContext?.Session.SetString("AccessToken", loginResponse.AccessToken);
                    _httpContextAccessor.HttpContext?.Session.SetString("Role", loginResponse.Role);
                    return RedirectToPage("/FootballPlayers/Index");
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Account is not found! Please check email and password again.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }
            return Page();
        }
    }

    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// Response model after logged in successfully
    /// </summary>
    public class AccountResponse
    {
        public int AccId { get; set; }

        public string? EmailAddress { get; set; }

        public string Description { get; set; } = null!;

        public string? Role { get; set; } 

        public string AccessToken { get; set; } = null!;
    }
}