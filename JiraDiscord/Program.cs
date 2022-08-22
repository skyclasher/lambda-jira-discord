using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using JiraDiscord.Models.Jira;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: LambdaSerializer(typeof(SourceGeneratorLambdaJsonSerializer<JiraDiscord.HttpApiJsonSerializerContext>))]
namespace JiraDiscord
{

	[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
	[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
	public partial class HttpApiJsonSerializerContext : JsonSerializerContext
	{
	}

	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}

		public async Task<APIGatewayHttpApiV2ProxyResponse> Handler(APIGatewayHttpApiV2ProxyRequest apiProxyEvent, ILambdaContext context)
		{
			try
			{

				Console.WriteLine(apiProxyEvent.Body); // Keep this log for now to capture created/close sprint later
				JiraBody jiraBody = JsonSerializer.Deserialize<JiraBody>(apiProxyEvent.Body)!;
				JiraEvent? jiraEvent = JiraParser.Parse(jiraBody);

				if (jiraEvent != null && !string.IsNullOrEmpty(jiraEvent.EventTypeLabel))
				{
					string title = $"{jiraEvent.Key}: {jiraEvent.Summary}";
					string desc = $"**{jiraEvent.EventTypeLabel}**\n{jiraEvent.Description}";
					string discordDetail = apiProxyEvent.QueryStringParameters["proxy"];
					string[] discordDetails = discordDetail.Split("/");

					bool isCallSuccess = await DiscordWebhook.SendDiscordWebhook(discordDetails[0], discordDetails[1], title, jiraEvent.Url, desc, jiraEvent.Author, jiraEvent.Color);
					if (!isCallSuccess)
					{
						return new APIGatewayHttpApiV2ProxyResponse
						{
							Body = "Call to discord server is unccessfull",
							StatusCode = 501,
						};
					}
				}

				return new APIGatewayHttpApiV2ProxyResponse
				{
					StatusCode = 200,
				};
			}
			catch (Exception ex)
			{
				return new APIGatewayHttpApiV2ProxyResponse
				{
					Body = ex.Message,
					StatusCode = 500,
				};
			}

		}
	}
}