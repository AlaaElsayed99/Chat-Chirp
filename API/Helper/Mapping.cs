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
            CreateMap<Message,MessageDTo>()
                .ForMember(d=>d.SenderPhotoUrl, o=>o.MapFrom(src=>src.Sender.Photos.
                FirstOrDefault(x=>x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(src => src.Recipient.Photos.
                FirstOrDefault(x => x.IsMain).Url));
        }
        
    }
}
