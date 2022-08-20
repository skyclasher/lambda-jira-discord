using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class JiraComment
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("body")]
		public string? Body { get; set; }

		[JsonPropertyName("updateAuthor")]
		public CommentAuthor? UpdateAuthor { get; set; }
	}
}
