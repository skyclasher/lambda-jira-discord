using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class ChangeLog
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("items")]
		public List<ChangeLogItem>? ChangeLogItem { get; set; }
	}
}
