using PE.Core.Contracts;

namespace PE.Core
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {

        IAccountRepository AccountRepository { get; }

        IFootballClubRepository FootballClubRepository { get; }

        IFootballPlayerRepository FootballPlayerRepository { get; }

        void Commit();

        Task CommitAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
