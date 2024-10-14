using System.ComponentModel.DataAnnotations;

namespace PE.Core.Dtos
{
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
