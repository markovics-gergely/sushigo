using AutoMapper;
using shared.dal.Models;
using shop.bll.Infrastructure.ViewModels;
using shop.bll.ValueConverters;
using shop.dal.Domain;

namespace shop.bll.MappingProfiles
{
    public class DeckProfile : Profile
    {
        public DeckProfile() {
            CreateMap<Deck, DeckViewModel>()
                .ForMember(d => d.CardTypes, opt => opt.MapFrom(v => v.Cards.Select(c => c.CardType)))
                .ForMember(d => d.ImagePath, opt => opt.ConvertUsing<ImageDisplayUrlConverter, string>(src => src.ImagePath));
            CreateMap<Deck, DeckItemViewModel>()
                .ForMember(d => d.MinPlayer, opt => opt.MapFrom(v => v.DeckType.GetMinPlayer()))
                .ForMember(d => d.MaxPlayer, opt => opt.MapFrom(v => v.DeckType.GetMaxPlayer()))
                .ForMember(d => d.ImagePath, opt => opt.ConvertUsing<ImageDisplayUrlConverter, string>(src => src.ImagePath));
        }
    }
}
