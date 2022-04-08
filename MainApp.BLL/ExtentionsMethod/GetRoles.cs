using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MainApp.BLL.ExtentionsMethod
{

    public static class ExtentionsMethod
    {

        public static List<string> GetRoles(this ClaimsIdentity identity)
        {
            return identity.Claims
                           .Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value)
                           .ToList();
        }
    }
}
