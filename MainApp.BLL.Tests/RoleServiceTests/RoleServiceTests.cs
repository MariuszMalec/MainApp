using FluentAssertions;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class RoleServiceTests : IClassFixture<ApplicationRoleSeedDataFixture>
    {
        ApplicationRoleSeedDataFixture _fixture;
        public RoleServiceTests(ApplicationRoleSeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetRoles_ShoudReturnRoles_WhenExist()
        {
            // Arrange
            var roles = _fixture.RoleContext.Roles;

            // Act
            var result = roles.Count();

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}
