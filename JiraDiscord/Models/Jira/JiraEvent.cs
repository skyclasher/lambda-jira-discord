namespace JiraDiscord.Models.Jira
{
	public class JiraEvent
	{
		public string? Key { get; set; }
		public string? Url { get; set; }
		public string? Summary { get; set; }
		public string? EventTypeLabel { get; set; }
		public string? Description { get; set; }
		public string? Author { get; set; }
		public int Color { get; set; }
	}
}
