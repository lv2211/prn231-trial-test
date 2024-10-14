using AutoMapper;
using PE.Core.Dtos;
using PE.Infrastructure;

namespace PE.Service
{
    public class MappingProfileExtension : Profile
    {
        public MappingProfileExtension()
        {
            CreateMap<PremierLeagueAccount, SigninAccountResponse>();

            CreateMap<FootballPlayer, FootballPlayerResponse>()
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => src.FootballClub.ClubName));

            CreateMap<CreateFootballPlayerRequest, FootballPlayer>();
            CreateMap<UpdateFootballPlayerRequest, FootballPlayer>();
        }
    }
}
