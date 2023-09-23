using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Moq;
using shared.dal.Models;
using user.bll.Infrastructure;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.DataTransferObjects;
using user.dal.Domain;
using user.test.MockData;

namespace user.test
{
    public class UserTest
    {
        [Fact]
        public async Task CreateUserAsync()
        {
            // Arrange
            var userdto = new RegisterUserDTO
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Password = "@TestPW1234",
                ConfirmedPassword = "@TestPW1234"
            };
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<ApplicationUser>(userdto)).Returns(new ApplicationUser
            {
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu"
            });
            var userManager = RepositoryMockData.MockUserManager;
            userManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), userdto.Password)).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), RoleTypes.Classic)).Returns(Task.FromResult(IdentityResult.Success));
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet<ApplicationUser>();
            userRepo.Setup(r => r.Update(It.Is<ApplicationUser>(u => u.DeckClaims.Contains(DeckType.SushiGo) && u.UserName == "Test")));
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            unitOfWork.Setup(u => u.Save());

            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                userManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                RepositoryMockData.MockMediator.Object
                );
            var command = new CreateUserCommand(userdto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);
            userManager.Verify(u => u.CreateAsync(It.Is<ApplicationUser>(au => au.UserName == "Test"), userdto.Password), Times.Once());
            userManager.Verify(u => u.AddToRoleAsync(It.Is<ApplicationUser>(au => au.UserName == "Test"), RoleTypes.Classic), Times.Once());
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == "Test")), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
        }
    }
}