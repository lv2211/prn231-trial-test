using PE.Core.Contracts;
using PE.Infrastructure.Databases;

namespace PE.Infrastructure.Repositories
{
    public class AccountRepository : Repository<PremierLeagueAccount>, IAccountRepository
    {
        public AccountRepository(EnglishPremierLeague2024DbContext dbContext) : base(dbContext)
        {
        }
    }
}
