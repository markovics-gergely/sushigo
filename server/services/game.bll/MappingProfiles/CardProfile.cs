using AutoMapper;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;

namespace game.bll.MappingProfiles
{
    public class CardProfile : Profile
    {
        public CardProfile() {
            CreateMap<HandCard, HandCardViewModel>();
            CreateMap<Hand, HandViewModel>();

            CreateMap<BoardCard, BoardCardViewModel>();
            CreateMap<Board, BoardViewModel>()
                .ForMember(b => b.Cards, s => s.MapFrom(b => b.Cards));
        }
    }
}
