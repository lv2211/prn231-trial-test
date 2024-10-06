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
        }
    }
}
