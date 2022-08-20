using System.Text.Json.Serialization;

namespace JiraDiscord.Models.Jira
{
	public class ChangeLogItem
	{
		[JsonPropertyName("field")]
		public string? Field { get; set; }

		[JsonPropertyName("fieldtype")]
		public string? FieldType { get; set; }

		[JsonPropertyName("fieldId")]
		public string? FieldId { get; set; }

		[JsonPropertyName("from")]
		public string? From { get; set; }

		[JsonPropertyName("fromString")]
		public string? FromString { get; set; }

		[JsonPropertyName("to")]
		public string? ItemTo { get; set; }

		[JsonPropertyName("toString")]
		public string? ItemToString { get; set; }
	}
}
