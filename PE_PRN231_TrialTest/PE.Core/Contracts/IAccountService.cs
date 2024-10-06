using PE.Core.Dtos;

namespace PE.Core.Contracts
{
    public interface IAccountService
    {
        Task<SigninAccountResponse?> AutheticateUser(string email, string password);
    }
}
