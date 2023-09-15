using Moq;
using shared.dal.Repository.Interfaces;
using System.Linq.Expressions;

namespace game.test.MockData
{
    public static class RepositoryMockData
    {
        public static Mock<IGenericRepository<TEntity>> GetMockRepositoryWithGet<TEntity>(List<TEntity>? mockGet = default) where TEntity : class
        {
            var mockRepository = new Mock<IGenericRepository<TEntity>>();
            mockRepository.Setup(r => r.Get(
                    It.IsAny<Expression<Func<TEntity, bool>>>(),
                    It.IsAny<Func<IQueryable<TEntity>, IQueryable<TEntity>>>(),
                    It.IsAny<string>()
                )).Returns(mockGet ?? new List<TEntity>());
            return mockRepository;
        }
    }
}
