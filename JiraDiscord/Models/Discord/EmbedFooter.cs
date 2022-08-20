using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Discord
{
	public class EmbedFooter
	{
		[JsonPropertyName("text")]
		public string? Text { get; set; }
	}
}
