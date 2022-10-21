using Microsoft.AspNetCore.Authentication;

namespace Tracking.Authentication.ApiKey
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
	{
		public const string AuthenticationScheme = "ApiKey";

		public string Realm { get; set; }

		public string Header { get; set; } = "ApiKey";

		public string QueryParam { get; set; } = "api_key";

		public string ApiKey { get; set; }
	}
}
