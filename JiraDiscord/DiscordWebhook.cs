using JiraDiscord.Models.Discord;
using RestSharp;

namespace JiraDiscord
{
	public class DiscordWebhook
	{
		// the post to Discord will reject the hook unless it includes valid User-Agent
		static readonly string USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		// https://discordapp.com/developers/docs/resources/channel#embed-limits
		static readonly int MAX_DESCRIPTION_LENGTH = 2048;


		static readonly string? DISCORD_ID = Environment.GetEnvironmentVariable("discord_id");
		static readonly string? DISCORD_TOKEN = Environment.GetEnvironmentVariable("discord_token");

		public static async Task<bool> SendDiscordWebhook(string title, string? url, string description, string? author, int color)
		{
			if (string.IsNullOrEmpty(DISCORD_ID) || string.IsNullOrEmpty(DISCORD_TOKEN))
			{
				Console.WriteLine("Discord id or token is not defined in the lambda enviroment variable.");
				return false;
			}

			var client = new RestClient($"https://discordapp.com/api/webhooks/{DISCORD_ID}/{DISCORD_TOKEN}");
			var request = new RestRequest();
			request.AddHeader("User-Agent", USER_AGENT);
			request.AddJsonBody(ConstructBodyString(title, url, description, author, color));
			var response = await client.ExecutePostAsync(request);

			if (response.IsSuccessful)
			{
				client.Dispose();
				return true;
			}
			else
			{
				Console.WriteLine($"{response.StatusCode} {response.Content}");
				Console.WriteLine($"{response.ErrorMessage} {response.ErrorException}");
				client.Dispose();
				return false;
			}
		}

		/**
		  * {
		  *   "username": "JIRA",
		  *   "avatar_url": "https://wiki.jenkins-ci.org/download/attachments/2916393/headshot.png",
		  *   "embeds": [{
		  *     "title": "This is the Title",
		  *     "url": "https://example.com",
		  *     "description": "This is a description that supports markdown txt",
		  *     "color": 1681177,
		  *     "footer": {
		  *       "text": "This is a footer"
		  *      }
		  *    }]
		  * }
		  */
		private static DiscordEmbedMsg ConstructBodyString(string title, string? url, string description, string? author, int color)
		{
			return new DiscordEmbedMsg()
			{
				Username = "JIRA",
				AvatarUrl = "https://a.slack-edge.com/ae7f/plugins/jira/assets/service_512.png",
				Embeds = new List<Embed>()
				{
					new Embed()
					{
						Title = title,
						Url = url,
						Description = Truncate(description, MAX_DESCRIPTION_LENGTH),
						Color = color,
						Footer = new EmbedFooter()
						{
							Text = author
						}
					}
				}
			};
		}

		private static string? Truncate(string? value, int maxLength)
		{
			if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
			{
				return value.Substring(0, maxLength);
			}
			return value;
		}
	}
}
