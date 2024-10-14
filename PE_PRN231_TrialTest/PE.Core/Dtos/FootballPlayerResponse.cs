namespace PE.Core.Dtos
{
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
