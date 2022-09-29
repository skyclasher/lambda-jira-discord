using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using JiraDiscord.Models.Jira;
using System.Text.RegularExpressions;

namespace JiraDiscord.Helper
{
	public static class GenericHelper
	{
		private static string _host = string.Empty;
		private static AmazonDynamoDBClient _client = new AmazonDynamoDBClient();

		public static string Host()
		{
			return _host;
		}

		public static string ConstructIssueUrl(string url, string key)
		{
			Uri uri = new Uri(url);
			_host = uri.Host;

			return $"https://{_host}/browse/{key}";
		}

		public static string ConstructSprintUrl(string url, string projectKey, int boardId)
		{
			if (string.IsNullOrEmpty(projectKey))
			{
				Console.WriteLine("Project Key is empty!");
				return string.Empty;
			}
			Uri uri = new Uri(url);
			_host = uri.Host;

			return $"https://{_host}/jira/software/projects/{projectKey}/boards/{boardId}";
		}

		public static string GetSubstringByString(string a, string b, string c)
		{
			return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
		}

		public static async Task<string> ResolveUser(string text)
		{
			if (!string.IsNullOrEmpty(_host))
			{
				if (text.Contains("~accountid"))
				{
					AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
					_client = new AmazonDynamoDBClient(clientConfig);
					DynamoDBContext context = new DynamoDBContext(_client);

					var pattern = @"\[(~accountid:.*?)\]";
					var matches = Regex.Matches(text, pattern);

					foreach (Match m in matches)
					{
						try
						{
							string accountId = GetSubstringByString("[~accountid:", "]", m.Value);
							JiraUser? jiraUser = await context.LoadAsync<JiraUser?>(accountId);

							if (jiraUser == null)
							{
								jiraUser = await JiraRestApi.GetJiraUserByAccountId(_host, accountId);
								if (jiraUser != null)
								{
									await context.SaveAsync<JiraUser>(jiraUser);
								}
							}

							if (jiraUser != null)
								text = text.Replace($"[~accountid:{accountId}]", jiraUser.DisplayName);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Error in ResolveUser : {ex.Message}");
						}
					}
				}
			}
			return text;
		}
	}
}
