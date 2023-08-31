using FluentAssertions;
using Moq;
using Tracking.Models;
using Tracking.Repositories;
using Tracking.Services;

namespace TrackingTests.UserServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IRepository<User>> _userMockRepo = new Mock<IRepository<User>>();

        public UserServiceTests()
        {
            _sut = new UserService(_userMockRepo.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            _userMockRepo.Setup(x => x.GetAll()).ReturnsAsync(GetUsers());

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().NotBeNullOrEmpty();
            users.Should().HaveCount(1);
            users.Select(u => u.Email).FirstOrDefault().Should().Be("test@gmail.com");
        }

        [Fact]
        public async Task Get_ShoudReturnTrue_WhenUserExist()
        {
            // Arrange
            var userId = 2;
            var userName = "";
            var userDto = new User
            {
                Id = 2,
                FirstName = userName
            };
            _userMockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(userDto);

            // Act
            var user = await _sut.Get(userId);

            // Assert
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task Get_ShoudReturnFalse_WhenUserNotExist()
        {
            // Arrange
            var userId = 1;//nie ma tego uzytkownika
            _userMockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new User() { });

            // Act
            var user = await _sut.Get(userId);

            // Assert
            Assert.Null(user.Email);
        }

        [Fact]
        public async Task Delete_ShoudReturnFalse_WhenUserNotExist()
        {
            // Arrange

            // Act
            await _sut.Delete(1);

            // Assert
            _userMockRepo.Verify(x => x.Delete(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShoudReturnTrue_WhenIsRun()
        {
            // Arrange
            var userDto = new User
            {
                Id = 1,
                FirstName = "Test"
            };

            // Act
            await _sut.Update(userDto);

            // Assert
            _userMockRepo.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        }

        private List<User> GetUsers()
        {
            var sessions = new List<User>();
            sessions.Add(new User()
            {
                CreatedDate = new DateTime(2016, 7, 2),
                Id = 1,
                FirstName = "test",
                LastName = "test",
                Email = "test@gmail.com"
            });
            return sessions;
        }
    }
}
