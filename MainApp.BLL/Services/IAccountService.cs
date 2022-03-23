using MainApp.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public interface IAccountService
    {
        Task<LoginResult> ValidateUser(string userName, string password);
    }
}
