using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class IssueField
	{
		[JsonPropertyName("statuscategorychangedate")]
		public string? StatusCategoryChangeDate { get; set; }

		[JsonPropertyName("issuetype")]
		public IssueType? IssueType { get; set; }

		[JsonPropertyName("summary")]
		public string? Summary { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("creator")]
		public IssueAuthor? Author { get; set; }
	}
}
