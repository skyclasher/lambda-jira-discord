using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using JiraDiscord.Helper;
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
				// Keep this log for now to capture created/close sprint to find the bug on created sprint
				Console.WriteLine(JsonSerializer.Serialize(apiProxyEvent));
				Console.WriteLine(apiProxyEvent.Body);
				JiraBody jiraBody = JsonSerializer.Deserialize<JiraBody>(apiProxyEvent.Body)!;

				if (!apiProxyEvent.QueryStringParameters.ContainsKey("projectKey"))
				{
					Console.WriteLine("Project key or proxy is missing from path parameter!");
					return new APIGatewayHttpApiV2ProxyResponse
					{
						StatusCode = 200,
					};
				}
				string projectKey = apiProxyEvent.QueryStringParameters["projectKey"];

				JiraEvent? jiraEvent = JiraParser.Parse(jiraBody, projectKey);

				if (jiraEvent != null && !string.IsNullOrEmpty(jiraEvent.EventTypeLabel) && !string.IsNullOrEmpty(jiraEvent.Url))
				{
					string title = string.Empty;
					if (string.IsNullOrEmpty(jiraEvent.Key))
					{
						title = $"{jiraEvent.Summary}";
					}
					else
					{
						title = $"{jiraEvent.Key}: {jiraEvent.Summary}";
					}

					string desc = $"**{jiraEvent.EventTypeLabel}**\n{jiraEvent.Description}";

					bool isCallSuccess = await DiscordWebhook.SendDiscordWebhook(title, jiraEvent.Url, await GenericHelper.ResolveUser(desc), jiraEvent.Author, jiraEvent.Color);
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