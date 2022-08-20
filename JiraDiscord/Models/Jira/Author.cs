using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class Author
	{
		[JsonPropertyName("displayName")]
		public string? DisplayName { get; set; }
	}
}
