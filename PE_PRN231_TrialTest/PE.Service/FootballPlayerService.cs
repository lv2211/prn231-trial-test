using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PE.Core;
using PE.Core.Contracts;
using PE.Core.Dtos;
using PE.Infrastructure;

namespace PE.Service
{
    public class FootballPlayerService : IFootballPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FootballPlayerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddPlayer(CreateFootballPlayerRequest request)
        {
            try
            {
                var existedPlayer = await _unitOfWork.FootballPlayerRepository.FindByIdAsync(request.FootballPlayerId);
                if (existedPlayer is not null) return false;
                var player = _mapper.Map<FootballPlayer>(request);

                _unitOfWork.FootballPlayerRepository.Add(player);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeletePlayer(FootballPlayer player)
        {
            try
            {
                if (player is not null)
                {
                    _unitOfWork.FootballPlayerRepository.Delete(player);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<FootballPlayerResponse?> GetPlayer(string id)
        {
            var player = await _unitOfWork.FootballPlayerRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.FootballClub)
                .FirstOrDefaultAsync(p => p.FootballPlayerId == id);
            return _mapper.Map<FootballPlayerResponse>(player);
        }

        public async Task<FootballPlayer?> GetPlayerById(string id)
            => await _unitOfWork.FootballPlayerRepository.FindByIdAsync(id);

        public async Task<IEnumerable<FootballPlayerResponse>> GetPlayers()
        {
            var players = await _unitOfWork.FootballPlayerRepository.GetAll()
                .AsNoTracking()
                .Include(p => p.FootballClub)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FootballPlayerResponse>>(players);
        }

        public async Task<bool> UpdatePlayer(UpdateFootballPlayerRequest request)
        {
            try
            {
                var existedPlayer = await _unitOfWork.FootballPlayerRepository.FindByIdAsync(request.FootballPlayerId);
                if (existedPlayer is null) return false;

                _ = _mapper.Map(request, existedPlayer);
                _unitOfWork.FootballPlayerRepository.Update(existedPlayer);
                await _unitOfWork.CommitAsync();
                return true;    
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
    }
}
