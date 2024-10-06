using PE.Core.Contracts;
using PE.Infrastructure.Databases;

namespace PE.Infrastructure.Repositories
{
    public class FootballPlayerRepository : Repository<FootballPlayer>, IFootballPlayerRepository
    {
        public FootballPlayerRepository(EnglishPremierLeague2024DbContext dbContext) : base(dbContext)
        {
        }
    }
}
