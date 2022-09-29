using JiraDiscord.Models.Jira;
using RestSharp;
using RestSharp.Authenticators;

namespace JiraDiscord
{
	public static class JiraRestApi
	{
		public static async Task<JiraUser?> GetJiraUserByAccountId(string host, string accountId)
		{
			string? jiraEmail = Environment.GetEnvironmentVariable("jira_email");
			string? jirApiKey = Environment.GetEnvironmentVariable("jira_api_key");
			if (string.IsNullOrEmpty(jiraEmail) || string.IsNullOrEmpty(jirApiKey))
			{
				Console.WriteLine("Jira email or api key is not defined in the lambda enviroment variable.");
				return null;
			}

			var client = new RestClient($"https://{host}/rest/api/2/user")
			{
				Authenticator = new HttpBasicAuthenticator(jiraEmail, jirApiKey)
			};
			var request = new RestRequest();
			request.AddParameter("accountId", accountId);

			var response = await client.ExecuteGetAsync<JiraUser>(request);

			if (response.IsSuccessful)
			{
				client.Dispose();
				return response.Data;
			}
			else
			{
				Console.WriteLine($"{response.StatusCode} {response.Content}");
				Console.WriteLine($"{response.ErrorMessage} {response.ErrorException}");
				client.Dispose();
				return null;
			}
		}
	}
}
