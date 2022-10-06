using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using Moq;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MainAppUnitTests.TrainerTests
{
    public class UserServiceTests
    {
        private readonly UserService _sut; 
        private readonly Mock<IRepository<ApplicationUser>> _userMockRepo = new Mock<IRepository<ApplicationUser>>();

        public UserServiceTests()
        {
            _sut = new UserService(_userMockRepo.Object);
        }


        [Fact]
        public async Task GetAllUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            _userMockRepo.Setup(x => x.GetAll()).ReturnsAsync(GetUsersDto());

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAllUsers_ShoudReturnEmptyUsers_WhenUsersNotExist()
        {
            // Arrange
            _userMockRepo.Setup(x => x.GetAll()).ReturnsAsync(new List<ApplicationUser>() { });

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().BeEmpty();
        }

        [Fact]
        public async Task GetById_ShoudReturnTrue_WhenUserExist()
        {
            // Arrange
            var userId = 2;
            var userName = "";
            var userDto = new ApplicationUser
            {
                Id = 2,
                FirstName = userName
            };
            _userMockRepo.Setup(x => x.GetById(userId)).ReturnsAsync(userDto);

            // Act
            var user = await _sut.GetById(userId);

            // Assert
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task GetById_ShoudReturnFalse_WhenUserNotExist()
        {
            // Arrange
            var userId = 1;
            var userName = "";
            var userDto = new ApplicationUser
            {
                Id = 2,
                FirstName = userName
            };
            _userMockRepo.Setup(x => x.GetById(userId)).ReturnsAsync(userDto);

            // Act
            var user = await _sut.GetById(userId);

            // Assert
            Assert.NotEqual(userId, user.Id);
        }

        [Fact]
        public async Task Id_Set_DuplicateId_ThenReturnWrong()
        {
            // Arrange
            _userMockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetUsersDto());

            // Act
            var userDtos = await _sut.GetAll();
            var userD = userDtos.Select(x => x.Id).ToList().Count();    

            var result = userDtos.Select(x => x.Id)
                .DistinctBy(x => x).Count();
            
            // Assert
            Assert.Equal(userD, result);   
        }

        private List<ApplicationUser> GetUsersDto()
        {
            var sessions = new List<ApplicationUser>();
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2016, 7, 2),
                Id = 1,
                FirstName = "dsdsd"
            });
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2018, 7, 2),
                Id = 2,
                FirstName = "dsdsd"
            });
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2018, 7, 2),
                Id = 3,
                FirstName = "dsdsd"
            });
            return sessions;
        }
    }
}
