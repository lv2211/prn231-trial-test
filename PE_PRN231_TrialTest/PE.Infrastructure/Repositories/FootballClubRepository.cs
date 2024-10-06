using PE.Core.Contracts;
using PE.Infrastructure.Databases;

namespace PE.Infrastructure.Repositories
{
    public class FootballClubRepository : Repository<FootballClub>, IFootballClubRepository
    {
        public FootballClubRepository(EnglishPremierLeague2024DbContext dbContext) : base(dbContext)
        {
        }
    }
}
