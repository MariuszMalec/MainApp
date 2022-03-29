using MainApp.BLL.Context;
using MainApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountService(ApplicationDbContext plannerContext)
        {
            _dbContext = plannerContext;
        }

        public async Task<LoginResult> ValidateUser(string userName, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userName);

            if (user != null)
            {
                var decodeBasePassword = Base64EncodeDecode.Base64Decode(user.PasswordHash);

                if (password == decodeBasePassword)
                {

                    return new LoginResult { UserName = userName, RoleName = "user", Success = true };//TODO poprawid , dodac Rolename

                    if (user.Role != null)
                    {
                        return new LoginResult { UserName = userName, RoleName = user.Role.Name, Success = true };
                    }
                }
            }

            return new LoginResult { Success = false };
        }
    }
}
