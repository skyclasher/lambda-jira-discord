using Amazon.DynamoDBv2.DataModel;
using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{

	[DynamoDBTable("jira_user_mapping")]
	public class JiraUser
	{
		[DynamoDBHashKey("jira_account_id")]
		[JsonPropertyName("accountId")]
		public string? AccountId { get; set; }

		[DynamoDBIgnore]
		[JsonPropertyName("self")]
		public string? SelfUrl { get; set; }

		[DynamoDBProperty("jira_display_name")]
		[JsonPropertyName("displayName")]
		public string? DisplayName { get; set; }
	}
}
