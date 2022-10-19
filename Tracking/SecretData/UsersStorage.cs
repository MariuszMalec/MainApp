using System.Collections.Generic;
using Tracking.Models;

namespace Tracking.SecretData
{
    public class UsersStorage
    {
        public static List<AuthenticateModel> Users = new List<AuthenticateModel>
        {
            new AuthenticateModel {Email = "admin@example.com", Password = "admin" }
        };
    }
}
