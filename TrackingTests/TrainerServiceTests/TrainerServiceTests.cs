using FluentAssertions;
using Moq;
using Tracking.Models;
using Tracking.Repositories;
using Tracking.Services;

namespace TrackingTests.TrainerServiceTests
{
    public class TrainerServiceTests
    {
        private readonly TrainerService _sut;
        private readonly Mock<IRepository<Trainer>> _mockRepo = new Mock<IRepository<Trainer>>();

        public TrainerServiceTests()
        {
            _sut = new TrainerService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(GetTrainers());

            // Act
            var result = await _sut.GetAll();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Select(u => u.Email).FirstOrDefault().Should().Be("test@gmail.com");
        }

        [Fact]
        public async Task Get_ShoudReturnTrue_WhenUserExist()
        {
            // Arrange
            var userId = 2;
            var userName = "";
            var userDto = new Trainer
            {
                Id = 2,
                FirstName = userName
            };
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(userDto);

            // Act
            var result = await _sut.Get(userId);

            // Assert
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task Get_ShoudReturnFalse_WhenUserNotExist()
        {
            // Arrange
            var userId = 1;//nie ma tego uzytkownika
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new Trainer() { });

            // Act
            var result = await _sut.Get(userId);

            // Assert
            Assert.Null(result.Email);
        }

        [Fact]
        public async Task Delete_ShoudReturnFalse_WhenUserNotExist()
        {
            // Arrange

            // Act
            await _sut.Delete(1);

            // Assert
            _mockRepo.Verify(x => x.Delete(It.IsAny<Trainer>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShoudReturnTrue_WhenIsRun()
        {
            // Arrange
            var userDto = new Trainer
            {
                Id = 1,
                FirstName = "Test"
            };

            // Act
            await _sut.Update(userDto);

            // Assert
            _mockRepo.Verify(x => x.Update(It.IsAny<Trainer>()), Times.Once);
        }

        private List<Trainer> GetTrainers()
        {
            var sessions = new List<Trainer>();
            sessions.Add(new Trainer()
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
