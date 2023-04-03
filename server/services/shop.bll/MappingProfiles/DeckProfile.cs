using AutoMapper;
using shop.bll.Infrastructure.ViewModels;
using shop.dal.Domain;

namespace shop.bll.MappingProfiles
{
    public class DeckProfile : Profile
    {
        public DeckProfile() {
            CreateMap<Deck, DeckViewModel>()
                .ForMember(d => d.CardTypes, opt => opt.MapFrom(v => v.Cards.Select(c => c.CardType)))
                .ReverseMap();
        }
    }
}
