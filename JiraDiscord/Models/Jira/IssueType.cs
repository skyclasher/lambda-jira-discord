using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class IssueType
	{
		[JsonPropertyName("statuscategorychangedate")]
		public DateTime? StatusCategoryChangeDate { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }
	}
}
