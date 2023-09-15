using game.bll.Infrastructure.Commands.Card.Utils.Implementations;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using game.test.MockData;
using Moq;
using System.Linq.Expressions;

namespace game.test
{
    public class AfterTurnTest
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
            mockBoardCardRepository.Verify(p => p.Insert(It.IsAny<BoardCard>()), Times.Once());
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
    }
}
