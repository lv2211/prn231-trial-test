namespace PE.Core.Dtos
{
    public class SigninAccountResponse
    {
        public int AccId { get; set; }

        public string? EmailAddress { get; set; }

        public string Description { get; set; } = null!;

        //public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = null!;
    }
}
