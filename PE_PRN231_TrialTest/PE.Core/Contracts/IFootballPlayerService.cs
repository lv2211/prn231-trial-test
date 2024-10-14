using PE.Core.Dtos;
using PE.Infrastructure;

namespace PE.Core.Contracts
{
    public interface IFootballPlayerService
    {
        Task<IEnumerable<FootballPlayerResponse>> GetPlayers();

        Task<FootballPlayerResponse?> GetPlayer(string id);

        Task<FootballPlayer?> GetPlayerById(string id);

        Task<bool> AddPlayer(CreateFootballPlayerRequest request);

        Task<bool> UpdatePlayer(UpdateFootballPlayerRequest request);

        Task<bool> DeletePlayer(FootballPlayer player);
    }
}
