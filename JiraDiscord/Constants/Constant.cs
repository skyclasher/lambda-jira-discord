namespace JiraDiscord.Constants
{
	public static class Constant
	{
		public static class JiraEvent
		{
			public static readonly string IssueCreated = "jira:issue_created";
			public static readonly string IssueUpdated = "jira:issue_updated";
			public static readonly string CommentCreated = "comment_created";
			public static readonly string CommentUpdated = "comment_updated";
			public static readonly string SprintStarted = "sprint_started";
			public static readonly string SprintUpdated = "sprint_updated";
			public static readonly string SprintClosed = "sprint_closed";

			public static readonly List<string> SprintEvent = new() { SprintStarted, SprintClosed };
		}
	}
}
