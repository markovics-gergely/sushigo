using AutoMapper;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.Domain;

namespace lobby.bll.MappingProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile() {
            CreateMap<Message, MessageViewModel>()
                .ForMember(m => m.DateTime, opt => opt.MapFrom(v => v.DateTime.ToString("u").Replace(" ", "T")));
            CreateMap<MessageDTO, Message>();
        }
    }
}
