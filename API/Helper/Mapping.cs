using API.Extensions;
using AutoMapper;

namespace API.Helper
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, AppUserDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.
                MapFrom(src => src.Photos.
                FirstOrDefault(s => s.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
                
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
        }
        
    }
}
