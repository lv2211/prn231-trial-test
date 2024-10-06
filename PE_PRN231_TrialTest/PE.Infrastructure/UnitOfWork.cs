using PE.Core;
using PE.Core.Contracts;
using PE.Infrastructure.Databases;

namespace PE.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnglishPremierLeague2024DbContext _dbContext;

        public UnitOfWork(
            EnglishPremierLeague2024DbContext dbContext,
            IAccountRepository accountRepository,
            IFootballClubRepository footballClubRepository,
            IFootballPlayerRepository footballPlayerRepository)
        {
            _dbContext = dbContext;
            AccountRepository = accountRepository;
            FootballClubRepository = footballClubRepository;
            FootballPlayerRepository = footballPlayerRepository;
        }

        public IAccountRepository AccountRepository { get; }

        public IFootballClubRepository FootballClubRepository { get; }

        public IFootballPlayerRepository FootballPlayerRepository { get; }

        public void Commit() => _dbContext.SaveChanges();

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _dbContext.Dispose();

        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
