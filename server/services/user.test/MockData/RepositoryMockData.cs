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

        public static Mock<RoleManager<ApplicationRole>> MockRoleManager { get => GetMockRoleManager(); }
        public static Mock<UserManager<ApplicationUser>> MockUserManager { get => GetMockUserManager(); }
        public static Mock<IFileConfigurationService> MockFileConfigurationService { get => new(); }
        public static Mock<IFileRepository> MockFileRepository { get => new(); }
        public static Mock<IMediator> MockMediator { get => new(); }
        public static Mock<IMapper> MockMapper { get => new(); }
        public static Mock<IUnitOfWork> MockUnitOfWork { get => new(); }
        private static Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        public static Mock<RoleManager<ApplicationRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return new Mock<RoleManager<ApplicationRole>>(
                         roleStore.Object, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

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
