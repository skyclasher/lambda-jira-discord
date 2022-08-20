using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class JiraIssue
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("self")]
		public string? IssueUrl { get; set; }

		[JsonPropertyName("key")]
		public string? Key { get; set; }

		[JsonPropertyName("fields")]
		public IssueField? IssueField { get; set; }
	}
}
