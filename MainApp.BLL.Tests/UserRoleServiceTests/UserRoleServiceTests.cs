using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using MainApp_BLL.Tests.RoleServiceTests;
using Moq;

namespace MainApp_BLL.Tests.UserRoleServiceTests
{
    public class UserRoleServiceTests : IClassFixture<SeedDataFixture>
    {

        private readonly Mock<IRepositoryService<ApplicationUserRoleView>> _repoService = new Mock<IRepositoryService<ApplicationUserRoleView>>();

        private readonly UserRoleService _sut;

        public UserRoleServiceTests(SeedDataFixture fixture)
        {
            _sut = new UserRoleService(fixture.context);
        }

        [Fact]
        public async Task GetById_ShoudReturnUserRole_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new ApplicationUserRoleView() {
                Id = 1,
                FirstName = "Test",
                LastName = "test",
                RoleId = 1,
                UserId = 1,
                UserRole = "Test"
            });

            // Act
            var roles = await _sut.GetById(1);

            var result = roles.FirstName;

            // Assert
            result.Should().Be("Test");
        }

        [Fact]
        public async Task GetAllUserRoles_ShoudReturnRoles_WhenExist()
        {
            // Arrange
            _repoService.Setup(x => x.GetAll()).ReturnsAsync(GetUserRoles());

            // Act
            var roles = await _sut.GetAll();

            // Assert
            roles.Should().NotBeEmpty();
            roles.Should().HaveCount(1);
            roles.Where(r=>r.FirstName == "Test").Select(x=>x.FirstName).FirstOrDefault().Should().Be("Test");
        }

        private IEnumerable<ApplicationUserRoleView> GetUserRoles()
        {
            var userroles = new List<ApplicationUserRoleView>();
            userroles.Add(new ApplicationUserRoleView() 
            { 
                Id = 1, 
                FirstName = "Test", 
                LastName = "test",
                RoleId = 1,
                UserId = 1,
                UserRole="Test"
            });
            return userroles;
        }
    }
}
