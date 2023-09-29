using IdentityModel;
using MassTransit.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using shared.dal.Models;
using shared.dal.Repository.Interfaces;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Security.Claims;
using user.bll.Infrastructure;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Infrastructure.Events;
using user.bll.Infrastructure.ViewModels;
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

        public static Expression<Func<T, bool>> AreEqual<T>(Expression<Func<T, bool>> expr)
        {
            return Match.Create<Expression<Func<T, bool>>>(t => t.ToString() == expr.ToString());
        }

        public static Expression<Func<T, bool>> AreNotEqual<T>(Expression<Func<T, bool>> expr)
        {
            return Match.Create<Expression<Func<T, bool>>>(t => t.ToString() != expr.ToString());
        }

        [Fact]
        public async Task EditUserAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userdto = new EditUserDTO
            {
                UserName = "TestEdit",
                FirstName = "TestEdit",
                LastName = "TestEdit"
            };
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
            };
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = "TestEdit",
                Name = "TestEdit TestEdit",
                Email = "Test@Test.hu"
            });
            var userRepo = new Mock<IGenericRepository<ApplicationUser>>();
            Expression<Func<ApplicationUser, bool>> editUserExpression = x => x.UserName == userdto.UserName && x.Id.ToString() != guid.ToString();
            Func<Expression, Expression, bool> eq = ExpressionEqualityComparer.Instance.Equals;
            userRepo.Setup(u => u.Get(
                    It.Is<Expression<Func<ApplicationUser, bool>>>(e => e.ToString().Contains("x.Id.ToString() != ")),
                    It.IsAny<Func<IQueryable<ApplicationUser>, IQueryable<ApplicationUser>>>(),
                    It.IsAny<string>()
                    )).Returns(new List<ApplicationUser>());
            userRepo.Setup(u => u.Get(
                    It.Is<Expression<Func<ApplicationUser, bool>>>(e => e.ToString().Contains("x.Id.ToString() == ")),
                    It.IsAny<Func<IQueryable<ApplicationUser>, IQueryable<ApplicationUser>>>(),
                    It.IsAny<string>()
                    )).Returns(new List<ApplicationUser> { user });
            userRepo.Setup(r => r.Update(It.Is<ApplicationUser>(u => u.UserName == userdto.UserName)));
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            unitOfWork.Setup(u => u.Save());
            var userManager = RepositoryMockData.MockUserManager;
            userManager.Setup(u => u.SetUserNameAsync(It.IsAny<ApplicationUser>(), userdto.UserName)).Returns(Task.FromResult(IdentityResult.Success));
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                userManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                RepositoryMockData.MockMediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var command = new EditUserCommand(userdto, new ClaimsPrincipal(identity));

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestEdit", result.UserName);
            Assert.Equal("TestEdit TestEdit", result.Name);
            userManager.Verify(u => u.SetUserNameAsync(It.Is<ApplicationUser>(au => au.UserName == userdto.UserName), userdto.UserName), Times.Once());
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == userdto.UserName)), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
        }

        [Fact]
        public async Task EditUserRoleAsync()
        {
            // Arrange
            var role = "Party";
            var guid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user }); ;
            userRepo.Setup(r => r.Update(It.Is<ApplicationUser>(u => u.UserName == user.UserName)));
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            unitOfWork.Setup(u => u.Save());
            var userManager = RepositoryMockData.MockUserManager;
            userManager.Setup(u => u.AddToRoleAsync(It.Is<ApplicationUser>(a => a.Id == user.Id), role)).Returns(Task.FromResult(IdentityResult.Success));
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                RepositoryMockData.MockMapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                userManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                RepositoryMockData.MockMediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new EditUserRoleDTO
            {
                Id = guid,
                Role = role
            };
            var command = new EditUserRoleCommand(dto);

            // Act
            await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            userManager.Verify(u => u.AddToRoleAsync(It.Is<ApplicationUser>(au => au.Id == user.Id), role), Times.Once());
        }

        [Fact]
        public async Task ClaimPartyAsync()
        {
            // Arrange
            var role = RoleTypes.Party;
            var guid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.PartyExp,
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user }); ;
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            var userManager = RepositoryMockData.MockUserManager;
            userManager.Setup(u => u.AddToRoleAsync(It.Is<ApplicationUser>(a => a.Id == user.Id), role)).Returns(Task.FromResult(IdentityResult.Success));
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = user.UserName,
                Name = user.LastName + " " + user.FirstName,
                Email = user.Email,
                Experience = user.Experience - RoleTypes.PartyExp
            });
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                userManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new PartyBoughtDTO
            {
                UserId = guid
            };
            var command = new ClaimPartyCommand(dto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            userManager.Verify(u => u.AddToRoleAsync(It.Is<ApplicationUser>(au => au.Id == user.Id), role), Times.Once());
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == user.UserName && au.Experience == 0)), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
            mapper.Verify(m => m.Map<UserViewModel>(user), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task ClaimDeckAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.DeckExp,
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user }); ;
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = user.UserName,
                Name = user.LastName + " " + user.FirstName,
                Email = user.Email,
                Experience = user.Experience - RoleTypes.DeckExp
            });
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                RepositoryMockData.MockUserManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new DeckBoughtDTO
            {
                UserId = guid,
                DeckType = DeckType.SushiGo
            };
            var command = new ClaimDeckCommand(dto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == user.UserName && au.Experience == 0 && au.DeckClaims.Count == 1 && au.DeckClaims.Contains(DeckType.SushiGo))), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
            mapper.Verify(m => m.Map<UserViewModel>(user), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task JoinLobbyAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var lobbyGuid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.DeckExp,
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user }); ;
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = user.UserName,
                Name = user.LastName + " " + user.FirstName,
                Email = user.Email,
                Experience = user.Experience - RoleTypes.DeckExp
            });
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                RepositoryMockData.MockUserManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new LobbyJoinedDTO
            {
                UserId = guid,
                LobbyId = lobbyGuid
            };
            var command = new JoinLobbyCommand(dto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == user.UserName && au.ActiveLobby == lobbyGuid)), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
            mapper.Verify(m => m.Map<UserViewModel>(user), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task JoinGameAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var gameGuid = Guid.NewGuid();
            var playerGuid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.DeckExp,
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user }); ;
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = user.UserName,
                Name = user.LastName + " " + user.FirstName,
                Email = user.Email,
                Experience = user.Experience - RoleTypes.DeckExp
            });
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                RepositoryMockData.MockUserManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new GameJoinedSingleDTO
            {
                UserId = guid,
                GameId = gameGuid,
                PlayerId = playerGuid
            };
            var command = new JoinGameCommand(dto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => au.UserName == user.UserName && au.ActiveLobby == null && au.ActiveGame == gameGuid && au.ActiveGamePlayer == playerGuid)), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Once());
            mapper.Verify(m => m.Map<UserViewModel>(user), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task EndGameAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var gameGuid = Guid.NewGuid();
            var playerGuid = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.DeckExp,
            };
            var history = new History
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                UserId = guid,
                Placement = 1,
                Point = 10
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user });
            var historyRepo = RepositoryMockData.GetMockRepositoryWithGet<History>();
            historyRepo.Setup(r => r.Insert(history));
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            unitOfWork.Setup(u => u.HistoryRepository).Returns(historyRepo.Object);
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RefreshUserEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var mapper = RepositoryMockData.MockMapper;
            mapper.Setup(m => m.Map<UserViewModel>(user)).Returns(new UserViewModel
            {
                UserName = user.UserName,
                Name = user.LastName + " " + user.FirstName,
                Email = user.Email,
                Experience = user.Experience - RoleTypes.DeckExp
            });
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                mapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                RepositoryMockData.MockUserManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var dto = new GameEndDTO
            {
                UserId = guid,
                GameName = "Test",
                Placement = 1,
                Points = 10,
            };
            var command = new EndGameCommand(dto);

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            userRepo.Verify(u => u.Update(It.Is<ApplicationUser>(au => 
                au.UserName == user.UserName &&
                au.ActiveGame == null &&
                au.ActiveGamePlayer == null &&
                au.Experience == RoleTypes.DeckExp + history.Point
                )), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Exactly(2));
            mapper.Verify(m => m.Map<UserViewModel>(user), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RemoveGameEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
            mediator.Verify(m => m.Publish(It.Is<RefreshHistoryEvent>(e => e.UserId == user.Id.ToString()), It.IsAny<CancellationToken>()), Times.Once());
            historyRepo.Verify(r => r.Insert(It.Is<History>(h => h.Name == history.Name && h.UserId == history.UserId && h.Placement == history.Placement && h.Point == history.Point)), Times.Once());
        }

        [Fact]
        public async Task RemoveUserAsync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var gameGuid = Guid.NewGuid();
            var playerGuid = Guid.NewGuid();
            var friendGuid = Guid.NewGuid();
            var friendUserId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@Test.hu",
                Experience = RoleTypes.DeckExp,
            };
            var history = new History
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                UserId = guid,
                Placement = 1,
                Point = 10
            };
            var friend = new Friend
            {
                Id = friendGuid,
                SenderId = guid,
                ReceiverId = friendUserId,
                Pending = false
            };
            var userRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<ApplicationUser> { user });
            var historyRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<History> { history });
            historyRepo.Setup(r => r.Delete(history));
            var friendsRepo = RepositoryMockData.GetMockRepositoryWithGet(new List<Friend> { friend });
            friendsRepo.Setup(f => f.Delete(friend));
            var unitOfWork = RepositoryMockData.MockUnitOfWork;
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            unitOfWork.Setup(u => u.HistoryRepository).Returns(historyRepo.Object);
            unitOfWork.Setup(u => u.FriendRepository).Returns(friendsRepo.Object);
            var mediator = RepositoryMockData.MockMediator;
            mediator.Setup(m => m.Publish(It.Is<RemoveFriendEvent>(e => e.SenderId == user.Id && e.ReceiverId == friendUserId), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var userManager = RepositoryMockData.MockUserManager;
            userManager.Setup(u => u.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { RoleTypes.Party }));
            userManager.Setup(u => u.RemoveFromRolesAsync(user, new List<string> { RoleTypes.Party })).Returns(Task.FromResult(IdentityResult.Success));
            userManager.Setup(u => u.DeleteAsync(user)).Returns(Task.FromResult(IdentityResult.Success));
            var userCommandHandler = new UserCommandHandler(
                unitOfWork.Object,
                RepositoryMockData.MockMapper.Object,
                RepositoryMockData.MockRoleManager.Object,
                userManager.Object,
                RepositoryMockData.MockFileConfigurationService.Object,
                RepositoryMockData.MockFileRepository.Object,
                mediator.Object
                );
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, guid.ToString()));
            var command = new RemoveUserCommand(new ClaimsPrincipal(identity));

            // Act
            var result = await userCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.Null(result);
            friendsRepo.Verify(f => f.Delete(It.Is<Friend>(fr => fr.SenderId == guid && fr.ReceiverId == friendUserId)), Times.Once());
            historyRepo.Verify(h => h.Delete(history), Times.Once());
            unitOfWork.Verify(p => p.Save(), Times.Exactly(2));
            mediator.Verify(m => m.Publish(It.Is<RemoveFriendEvent>(e => e.SenderId == guid && e.ReceiverId == friendUserId), It.IsAny<CancellationToken>()), Times.Once());
            userManager.Verify(u => u.GetRolesAsync(user), Times.Once());
            userManager.Verify(u => u.RemoveFromRolesAsync(user, new List<string> { RoleTypes.Party }), Times.Once());
            userManager.Verify(u => u.DeleteAsync(user), Times.Once());
        }
    }
}