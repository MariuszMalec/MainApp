using FluentAssertions;

namespace MainAppIntegtratedMvcTests
{
    public class UserContextTests : IClassFixture<ApplicationUserSeedDataFixture>
    {
        ApplicationUserSeedDataFixture _fixture;

        public UserContextTests(ApplicationUserSeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetUsers_ShoudReturnUsers_WhenUserExist()
        {
            // Arrange
            var users = _fixture.UserContext.Users;

            // Act
            var result = users.Count();

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void GetUser_ShoudReturnUser_WhenUserIsAdmin()
        {
            // Arrange
            var users = _fixture.UserContext.Users;

            // Act
            var result = users.FirstOrDefault();

            // Assert
            Assert.Equal("Admin@example.com",result.Email);
        }
    }
}
