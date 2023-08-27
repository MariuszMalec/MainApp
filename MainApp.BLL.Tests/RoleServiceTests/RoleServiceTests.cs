using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using Moq;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class RoleServiceTests : IClassFixture<SeedDataFixture>
    {

        private readonly Mock<IRepositoryService<ApplicationRoles>> _repoService = new Mock<IRepositoryService<ApplicationRoles>>();

        private readonly RoleService _sut;

        public RoleServiceTests(SeedDataFixture fixture)
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

        [Fact]
        public async Task Delete_ShoudDeleteRole_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<ApplicationRoles>())).ReturnsAsync(true);

            // Act
            var result = await _sut.Delete(1, new ApplicationRoles() { Name = "test", NormalizedName = "TEST" });
            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task Delete_ShoudNotDeleteRole_WhenNotExist()
        {
            // Arrange
            _repoService.Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<ApplicationRoles>())).ReturnsAsync(false);

            // Act
            var result = await _sut.Delete(3, new ApplicationRoles() { Id = 1, Name = "test", NormalizedName = "TEST" });
            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task Update_ShoudUpdateRole_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.Update(It.IsAny<int>() ,It.IsAny<ApplicationRoles>())).ReturnsAsync(true);

            // Act
            var result = await _sut.Update(1, new ApplicationRoles() { Name = "testupdate", NormalizedName = "TESTUPDATE" });
            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task Update_ShoudNotUpdateRole_WhenNotExist()
        {
            // Arrange
            _repoService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<ApplicationRoles>())).ReturnsAsync(false);

            // Act
            var result = await _sut.Update(3, new ApplicationRoles() { Id=1, Name = "testupdate", NormalizedName = "TESTUPDATE" });
            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task Insert_ShoudCreateRole_WhenNotExist()
        {
            // Arrange
            _repoService.Setup(x => x.Insert(It.IsAny<ApplicationRoles>())).ReturnsAsync(true);

            // Act
            var result = await _sut.Insert(new ApplicationRoles() { Id=2, Name="testCreation", NormalizedName="TESTCREATION"});
            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task Insert_ShoudNotCreateRole_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.Insert(It.IsAny<ApplicationRoles>())).ReturnsAsync(false);

            // Act
            var result = await _sut.Insert(new ApplicationRoles() { Id = 1, Name = "test", NormalizedName = "TEST" });
            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task GetById_ShoudReturnRole_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ApplicationRoles() { Id=1,Name="Test",NormalizedName="TEST"});

            // Act
            var roles = await _sut.GetById(1);

            var result = roles.Name;

            // Assert
            result.Should().Be("Test");
        }

        private IEnumerable<ApplicationRoles> GetRoles()
        {
            var roles = new List<ApplicationRoles>();
            roles.Add(new ApplicationRoles() { Id = 1, Name = "Test", NormalizedName = "TEST" });
            return roles;
        }
    }
}
