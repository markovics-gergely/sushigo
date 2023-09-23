using game.bll.Infrastructure.Commands.Card.Utils.Implementations;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using game.test.MockData;
using Moq;
using System.Linq.Expressions;

namespace game.test
{
    public class EndRoundTest
    {
        [Fact]
        public async Task SimpleAddPointTestAsync()
        {
            // Arrange
            var mockPlayers = DomainMockData.Players;
            var mockBoardCards = DomainMockData.BoardCards;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPlayerRepository = RepositoryMockData.GetMockRepositoryWithGet(mockPlayers);
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet(mockBoardCards);
            mockUnitOfWork.Setup(u => u.PlayerRepository).Returns(mockPlayerRepository.Object);
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            var simpleAddPoint = new SimpleAddPoint(mockUnitOfWork.Object);

            // Act
            await simpleAddPoint.CalculateEndRound(mockBoardCards.First(), 2);

            // Assert
            mockPlayerRepository.Verify(p => p.Get(
                It.IsAny<Expression<Func<Player, bool>>>(),
                    It.IsAny<Func<IQueryable<Player>, IQueryable<Player>>>(),
                    It.IsAny<string>()), Times.Once());
            mockPlayerRepository.Verify(p => p.Update(mockPlayers.First()), Times.Once());
            mockBoardCardRepository.Verify(p => p.Update(mockBoardCards.First()), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }

        [Fact]
        public async Task AddPointByDelegateTestAsync()
        {
            // Arrange
            var mockPlayers = DomainMockData.Players;
            var mockBoardCards = DomainMockData.BoardCards;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPlayerRepository = RepositoryMockData.GetMockRepositoryWithGet(mockPlayers);
            var mockBoardCardRepository = RepositoryMockData.GetMockRepositoryWithGet(mockBoardCards);
            mockUnitOfWork.Setup(u => u.PlayerRepository).Returns(mockPlayerRepository.Object);
            mockUnitOfWork.Setup(u => u.BoardCardRepository).Returns(mockBoardCardRepository.Object);
            var addPointByDelegate = new AddPointByDelegate(mockUnitOfWork.Object);

            // Act
            await addPointByDelegate.CalculateEndRound(mockBoardCards.First(), c => 1);

            // Assert
            mockPlayerRepository.Verify(p => p.Get(
                It.IsAny<Expression<Func<Player, bool>>>(),
                    It.IsAny<Func<IQueryable<Player>, IQueryable<Player>>>(),
                    It.IsAny<string>()), Times.Once());
            mockPlayerRepository.Verify(p => p.Update(mockPlayers.First()), Times.Once());
            mockBoardCardRepository.Verify(p => p.Update(mockBoardCards.First()), Times.Once());
            mockUnitOfWork.Verify(p => p.Save(), Times.Once());
        }
    }
}