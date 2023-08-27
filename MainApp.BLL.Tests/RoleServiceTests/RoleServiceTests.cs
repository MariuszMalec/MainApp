using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using Moq;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class RoleServiceTests : IClassFixture<RoleSeedDataFixture>
    {

        private readonly Mock<IRepositoryService<ApplicationRoles>> _repoService = new Mock<IRepositoryService<ApplicationRoles>>();

        private readonly RoleService _sut;

        public RoleServiceTests(RoleSeedDataFixture fixture)
        {
            _sut = new RoleService(fixture.context);
        }

        [Fact]
        public async Task GetAllRoles_ShoudReturnRoles_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.GetAll()).ReturnsAsync(GetRoles());

            // Act
            var roles = await _sut.GetAll();

            // Assert
            roles.Should().NotBeEmpty();
            roles.Should().HaveCount(1);
        }

        private IEnumerable<ApplicationRoles> GetRoles()
        {
            var roles = new List<ApplicationRoles>();
            roles.Add(new ApplicationRoles() { Id = 1, Name = "Test", NormalizedName = "TEST" });
            return roles;
        }
    }
}
