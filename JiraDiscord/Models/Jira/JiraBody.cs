using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class JiraBody
	{
		[JsonPropertyName("webhookEvent")]
		public string? WebhookEvent { get; set; }

		[JsonPropertyName("issue")]
		public JiraIssue? Issue { get; set; }

		[JsonPropertyName("comment")]
		public JiraComment? Comment { get; set; }

		[JsonPropertyName("changelog")]
		public ChangeLog? ChangeLog { get; set; }

		[JsonPropertyName("sprint")]
		public JiraSprint? Sprint { get; set; }
	}
}
