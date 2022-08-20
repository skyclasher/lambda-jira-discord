using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Discord
{
	public class Embed
	{
		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("url")]
		public string? Url { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("color")]
		public int Color { get; set; }

		[JsonPropertyName("footer")]
		public EmbedFooter? Footer { get; set; }
	}
}
