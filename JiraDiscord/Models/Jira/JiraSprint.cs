using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class JiraSprint
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("self")]
		public string? SelfUrl { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("startDate")]
		public string? StartDate { get; set; }

		[JsonPropertyName("endDate")]
		public string? EndDate { get; set; }

		[JsonPropertyName("originBoardId")]
		public int BoardId { get; set; } = 0;

		[JsonPropertyName("goal")]
		public string? Goal { get; set; }
	}
}
