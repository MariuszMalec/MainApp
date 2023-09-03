using FluentAssertions;
using MainApp.BLL.Enums;
using Moq;
using Tracking.Models;
using Tracking.Repositories;
using Tracking.Services;

namespace TrackingTests.TrackingServiceTests
{
    public class TrackingServiceTests
    {
        private readonly TrackingService _sut;
        private readonly Mock<IRepository<Event>> _mockRepo = new Mock<IRepository<Event>>();

        public TrackingServiceTests()
        {
            _sut = new TrackingService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ShoudReturnEvents_WhenUsersExist()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(GetEvents());

            // Act
            var result = await _sut.GetAll();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Select(u => u.Action).FirstOrDefault().Should().Be(ActivityActions.loggin.ToString());
        }

        [Fact]
        public async Task Get_ShoudReturnTrue_WhenEventExist()
        {
            // Arrange
            var eventId = 2;
            var eventDto = new Event
            {
                Id = 2,
                UserId = 1,
                Action = ActivityActions.loggin.ToString(),
                CreatedDate = DateTime.Now,
            };
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(eventDto);

            // Act
            var result = await _sut.Get(eventId);

            // Assert
            Assert.Equal(eventId, result.Id);
        }

        [Fact]
        public async Task Get_ShoudReturnFalse_WhenEventNotExist()
        {
            // Arrange
            var evntId = 1;//nie ma tego uzytkownika
            _mockRepo.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(new Event() { });

            // Act
            var result = await _sut.Get(evntId);

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
            _mockRepo.Verify(x => x.Delete(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShoudReturnTrue_WhenIsRun()
        {
            // Arrange
            var eventDto = new Event
            {
                Id = 2,
                UserId = 1,
                Action = ActivityActions.loggin.ToString(),
                CreatedDate = DateTime.Now,
            };

            // Act
            await _sut.Update(eventDto);

            // Assert
            _mockRepo.Verify(x => x.Update(It.IsAny<Event>()), Times.Once);
        }

        private List<Event> GetEvents()
        {
            var sessions = new List<Event>();
            sessions.Add(new Event()
            {
                CreatedDate = new DateTime(2016, 7, 2),
                Id = 2,
                UserId = 1,
                Action = ActivityActions.loggin.ToString(),
        });
            return sessions;
        }
    }
}
