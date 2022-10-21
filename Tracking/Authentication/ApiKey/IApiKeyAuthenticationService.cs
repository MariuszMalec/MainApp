using System.Threading.Tasks;

namespace Tracking.Authentication.ApiKey
{
    public interface IApiKeyAuthenticationService
	{
		Task<bool> IsValidApiKey(string apiKey);
	}
}