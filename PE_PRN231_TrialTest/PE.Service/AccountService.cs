using AutoMapper;
using PE.Core;
using PE.Core.Contracts;
using PE.Core.Dtos;

namespace PE.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, TokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<SigninAccountResponse?> AutheticateUser(string email, string password)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.GetAsync(a => a.EmailAddress == email && a.Password == password);
                if (account is not null)
                {
                    var accessToken = _tokenService.GenerateToken(account);
                    var response = _mapper.Map<SigninAccountResponse>(account);
                    response.AccessToken = accessToken;
                    return response;
                }
                return null;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
