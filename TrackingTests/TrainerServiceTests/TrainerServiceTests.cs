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
        public async Task GetAll_ShoudReturnTrainers_WhenUsersExist()
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
        public async Task Get_ShoudReturnTrue_WhenTrainerExist()
        {
            // Arrange
            var trainerId = 2;
            var trainerDto = new Trainer
            {
                Id = 2
            };
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(trainerDto);

            // Act
            var result = await _sut.Get(trainerId);

            // Assert
            Assert.Equal(trainerId, result.Id);
        }

        [Fact]
        public async Task Get_ShoudReturnFalse_WhenTrainerNotExist()
        {
            // Arrange
            var trainerId = 1;//nie ma tego uzytkownika
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new Trainer() { });

            // Act
            var result = await _sut.Get(trainerId);

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
            var trainerDto = new Trainer
            {
                Id = 1
            };

            // Act
            await _sut.Update(trainerDto);

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
