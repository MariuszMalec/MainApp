using Microsoft.AspNetCore.Identity;
using Moq;

namespace MainApp.Web.Tests.AccountControlerTests
{
    public static class MockUserManager
    {
        public static Mock<UserManager<TUser>> GetUserManager<TUser>()
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var passwordHasher = new Mock<IPasswordHasher<TUser>>();
            IList<IUserValidator<TUser>> userValidators = new List<IUserValidator<TUser>>
        {
            new UserValidator<TUser>()
        };
            IList<IPasswordValidator<TUser>> passwordValidators = new List<IPasswordValidator<TUser>>
        {
            new PasswordValidator<TUser>()
        };
            userValidators.Add(new UserValidator<TUser>());
            passwordValidators.Add(new PasswordValidator<TUser>());
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var userManager = new Mock<UserManager<TUser>>(store.Object, null, passwordHasher.Object, userValidators, passwordValidators, null, null, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return userManager;
        }
    }
}

