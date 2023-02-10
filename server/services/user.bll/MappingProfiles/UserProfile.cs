using AutoMapper;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Infrastructure.ViewModels;
using user.dal.Domain;

namespace user.bll.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, EditUserDTO>().ReverseMap();

            CreateMap<RegisterUserDTO, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(a => a.UserName.Trim()))
                .ForMember(u => u.FirstName, opt => opt.MapFrom(a => a.FirstName.Trim()))
                .ForMember(u => u.LastName, opt => opt.MapFrom(a => a.LastName.Trim()))
                .ForMember(u => u.Email, opt => opt.MapFrom(a => a.Email.Trim()))
                .ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(u => u.Name, opt => opt.MapFrom(a => a.GetFullName()))
                .ReverseMap();

            CreateMap<ApplicationUser, UserNameViewModel>()
                .ReverseMap();
        }
    }
}
