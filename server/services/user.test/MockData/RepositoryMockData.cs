using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using shared.dal.Configurations.Interfaces;
using shared.dal.Repository.Interfaces;
using System.Linq.Expressions;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;

namespace user.test.MockData
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

        public static Mock<RoleManager<ApplicationRole>> MockRoleManager { get; } = GetMockRoleManager();
        public static Mock<UserManager<ApplicationUser>> MockUserManager { get; } = GetMockUserManager();
        public static Mock<IFileConfigurationService> MockFileConfigurationService { get; } = new Mock<IFileConfigurationService>();
        public static Mock<IFileRepository> MockFileRepository { get; } = new Mock<IFileRepository>();
        public static Mock<IMediator> MockMediator { get; } = new Mock<IMediator>();
        public static Mock<IMapper> MockMapper { get; } = new Mock<IMapper>();
        public static Mock<IUnitOfWork> MockUnitOfWork { get; } = new Mock<IUnitOfWork>();
        private static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
        public static Mock<RoleManager<ApplicationRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            return new Mock<RoleManager<ApplicationRole>>(
                         roleStore.Object, null, null, null, null);

        }
        public static Mock<IdentityResult> MockIdentityResultSuccess { get { 
                var result = new Mock<IdentityResult>();
                result.Setup(r => r.Succeeded).Returns(true);
                result.Setup(r => r.Errors).Returns(new List<IdentityError>());
                return result;
            }
        }
        
    }
}
