namespace PE.Core.Dtos
{
    public class SigninRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
