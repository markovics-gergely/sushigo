using game.bll.Infrastructure.Commands.Card;
using game.bll.Infrastructure.Commands.Card.Utils.Implementations;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using game.test.MockData;
using Moq;
using shared.dal.Models.Types;
using System.Linq.Expressions;

namespace game.test
{
    public class EndTurnTest
    {
        [Fact]
        public async Task SimpleAddToBoardAsync()
        {
            // Arrange
            var mockPlayer = DomainMockData.Player;
            var mockHandCard = DomainMockData.HandCard;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet<BoardCard>();
            var mockHandCardRepository = RepositoryMockData.GetMockRepositoryWithGet<HandCard>();
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            mockUnitOfWork.Setup(u => u.HandCardRepository).Returns(mockHandCardRepository.Object);
            var simpleAddToBoard = new SimpleAddToBoard(mockUnitOfWork.Object);

            // Act
            await simpleAddToBoard.AddToBoard(mockPlayer, mockHandCard);

            // Assert
            mockHandCardRepository.Verify(p => p.Delete(mockHandCard), Times.Once());
            mockBoardCardRepository.Verify(p => p.Insert(It.Is<BoardCard>(b =>
                b.BoardId == mockPlayer.BoardId &&
                b.CardInfo.CardType == mockHandCard.CardInfo.CardType &&
                b.GameId == mockHandCard.GameId
            )), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }
        [Fact]
        public async Task NigiriAddToBoardAsync()
        {
            // Arrange
            var mockPlayer = DomainMockData.Player;
            var mockHandCard = DomainMockData.HandCard;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet<BoardCard>();
            var mockHandCardRepository = RepositoryMockData.GetMockRepositoryWithGet<HandCard>();
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            mockUnitOfWork.Setup(u => u.HandCardRepository).Returns(mockHandCardRepository.Object);
            var simpleAddToBoard = new SimpleAddToBoard(mockUnitOfWork.Object);

            // Act
            await simpleAddToBoard.AddNigiriToBoard(mockPlayer, mockHandCard);

            // Assert
            mockHandCardRepository.Verify(p => p.Delete(mockHandCard), Times.Once());
            mockBoardCardRepository.Verify(p => p.Insert(It.IsAny<BoardCard>()), Times.Once());
            mockBoardCardRepository.Verify(p => p.Get(
                It.IsAny<Expression<Func<BoardCard, bool>>>(),
                    It.IsAny<Func<IQueryable<BoardCard>, IQueryable<BoardCard>>>(),
                    It.IsAny<string>()), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }

        [Fact]
        public async Task UramakiEndTurnAsync()
        {
            // Arrange
            var mockPlayer = DomainMockData.Player;
            var mockHandCard = DomainMockData.HandCard;
            mockHandCard.CardInfo.CardType = CardType.Uramaki;
            mockHandCard.CardInfo.Point = 10;
            var mockUramakiBoardCards = DomainMockData.UramakiBoardCards;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet(mockUramakiBoardCards);
            var mockHandCardRepository = RepositoryMockData.GetMockRepositoryWithGet<HandCard>();
            var mockGameRepository = RepositoryMockData.GetMockRepositoryWithGet(new List<Game> { DomainMockData.Game });
            var mockPlayerRepository = RepositoryMockData.GetMockRepositoryWithGet(new List<Player> { DomainMockData.Player });
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            mockUnitOfWork.Setup(u => u.HandCardRepository).Returns(mockHandCardRepository.Object);
            mockUnitOfWork.Setup(u => u.GameRepository).Returns(mockGameRepository.Object);
            mockUnitOfWork.Setup(u => u.PlayerRepository).Returns(mockPlayerRepository.Object);
            var simpleAddToBoard = new SimpleAddToBoard(mockUnitOfWork.Object);
            var uramakiCommand = new UramakiCommand(mockUnitOfWork.Object, simpleAddToBoard);

            var startingPoint = mockPlayer.Points;

            // Act
            await uramakiCommand.OnEndTurn(mockPlayer, mockHandCard);

            // Assert
            mockBoardCardRepository.Verify(p => p.Get(
                It.IsAny<Expression<Func<BoardCard, bool>>>(),
                    It.IsAny<Func<IQueryable<BoardCard>, IQueryable<BoardCard>>>(),
                    It.IsAny<string>()), Times.Once());
            mockGameRepository.Verify(p => p.Get(
                It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Func<IQueryable<Game>, IQueryable<Game>>>(),
                    It.IsAny<string>()), Times.Once());
            mockPlayerRepository.Verify(p => p.Update(It.Is<Player>(p =>
                p.Id == mockPlayer.Id &&
                p.Points == startingPoint + 8
            )), Times.Once());
            mockBoardCardRepository.Verify(p => p.Delete(It.Is<BoardCard>(bc => bc.Id == mockUramakiBoardCards[0].Id)), Times.Exactly(mockUramakiBoardCards.Count));
            mockHandCardRepository.Verify(p => p.Delete(mockHandCard), Times.Once());
            mockGameRepository.Verify(p => p.Update(DomainMockData.Game), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }

        [Fact]
        public async Task TakeoutBoxEndTurnAsync()
        {
            // Arrange
            var mockPlayer = DomainMockData.Player;
            var mockHandCard = DomainMockData.HandCard;
            mockHandCard.CardInfo.CardIds?.Contains(DomainMockData.BoardCardId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet<BoardCard>();
            var mockHandCardRepository = RepositoryMockData.GetMockRepositoryWithGet<HandCard>();
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            mockUnitOfWork.Setup(u => u.HandCardRepository).Returns(mockHandCardRepository.Object);
            var simpleAddPoint = new SimpleAddPoint(mockUnitOfWork.Object);
            var takeoutBoxCommand = new TakeoutBoxCommand(mockUnitOfWork.Object, simpleAddPoint);

            // Act
            await takeoutBoxCommand.OnEndTurn(mockPlayer, mockHandCard);

            // Assert
            mockBoardCardRepository.Verify(p => p.Delete(DomainMockData.BoardCardId), Times.Once());
            mockBoardCardRepository.Verify(p => p.Insert(It.Is<BoardCard>(b =>
                b.BoardId == mockPlayer.BoardId &&
                b.CardInfo.CardType == CardType.TakeoutBox &&
                b.GameId == mockPlayer.GameId &&
                b.CardInfo.CustomTag == CardTagType.CONVERTED
            )), Times.Once());
            mockHandCardRepository.Verify(p => p.Delete(mockHandCard), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }

        [Fact]
        public async Task SpecialOrderEndTurnAsync()
        {
            // Arrange
            var mockPlayer = DomainMockData.Player;
            var mockHandCard = DomainMockData.HandCard;
            var mockBoardCard = DomainMockData.BoardCard;
            mockHandCard.CardInfo.CardIds?.Contains(DomainMockData.BoardCardId);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet(new List<BoardCard> { mockBoardCard });
            var mockHandCardRepository = RepositoryMockData.GetMockRepositoryWithGet<HandCard>();
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            mockUnitOfWork.Setup(u => u.HandCardRepository).Returns(mockHandCardRepository.Object);
            var simpleAddPoint = new SimpleAddPoint(mockUnitOfWork.Object);
            var noModification = new NoModification(mockUnitOfWork.Object);
            var specialOrderCommand = new SpecialOrderCommand(mockUnitOfWork.Object, simpleAddPoint, noModification);

            // Act
            await specialOrderCommand.OnEndTurn(mockPlayer, mockHandCard);

            // Assert
            mockBoardCardRepository.Verify(p => p.Insert(It.Is<BoardCard>(b =>
                b.BoardId == mockBoardCard.BoardId &&
                b.CardInfo.CardType == mockBoardCard.CardInfo.CardType &&
                b.GameId == mockBoardCard.GameId
            )), Times.Once());
            mockHandCardRepository.Verify(p => p.Delete(mockHandCard), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }
    }
}
