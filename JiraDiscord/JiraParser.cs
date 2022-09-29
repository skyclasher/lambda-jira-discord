using JiraDiscord.Constants;
using JiraDiscord.Helper;
using JiraDiscord.Models.Jira;

namespace JiraDiscord
{
	public static class JiraParser
	{
		public static JiraEvent? Parse(JiraBody jiraBody, string projectKey)
		{
			if (string.IsNullOrEmpty(jiraBody.WebhookEvent))
			{
				Console.WriteLine("Webhook Event Missing!");
				return null;
			}

			if (Constant.JiraEvent.SprintEvent.Contains(jiraBody.WebhookEvent))
			{
				if (jiraBody != null && jiraBody.Sprint != null && jiraBody.Sprint.SelfUrl != null && jiraBody.Sprint.BoardId != 0)
				{
					JiraEvent jiraEvent = new JiraEvent()
					{
						Url = GenericHelper.ConstructSprintUrl(jiraBody.Sprint.SelfUrl, projectKey, jiraBody.Sprint.BoardId),
						Summary = $"{projectKey}: {jiraBody.Sprint.Name}",
						EventTypeLabel = $"{jiraBody.Sprint.Name}"
					};
					StartSprint(jiraBody, ref jiraEvent, projectKey);
					return jiraEvent;
				}
				Console.WriteLine("Board Id or Self url is missing!");
				return null;
			}
			else if (jiraBody?.Issue?.IssueUrl == null || jiraBody.Issue.Key == null)
			{
				Console.WriteLine("Issue url or key is missing!");
				return null;
			}
			else
			{
				JiraEvent jiraEvent = new JiraEvent()
				{
					Key = jiraBody.Issue.Key,
					Url = GenericHelper.ConstructIssueUrl(jiraBody.Issue.IssueUrl, jiraBody.Issue.Key),
					Summary = jiraBody?.Issue?.IssueField?.Summary
				};

				if (string.Equals(jiraBody?.WebhookEvent, Constant.JiraEvent.IssueCreated))
				{
					CreateIssue(jiraBody, ref jiraEvent);
				}
				else if (string.Equals(jiraBody?.WebhookEvent, Constant.JiraEvent.IssueUpdated))
				{
					UpdateIssueStatus(jiraBody, ref jiraEvent);
				}
				else if (string.Equals(jiraBody?.WebhookEvent, Constant.JiraEvent.CommentCreated))
				{
					Comment(jiraBody, ref jiraEvent, "Comment Created");
				}
				else if (string.Equals(jiraBody?.WebhookEvent, Constant.JiraEvent.CommentUpdated))
				{
					Comment(jiraBody, ref jiraEvent, "Comment Updated");
				}
				else
				{
					Console.WriteLine("Event is not supported");
					return null;
				}
				return jiraEvent;
			}
		}

		private static void StartSprint(JiraBody? jiraBody, ref JiraEvent jiraEvent, string projectKey)
		{
			if (jiraBody?.WebhookEvent == Constant.JiraEvent.SprintStarted)
			{
				jiraEvent.Summary = $"{projectKey}: Sprint Started";
				jiraEvent.Description = jiraBody?.Sprint?.Goal;
				jiraEvent.Color = 12892909;
			}
			if (jiraBody?.WebhookEvent == Constant.JiraEvent.SprintClosed)
			{
				jiraEvent.Summary = $"{projectKey}: Sprint Closed";
				jiraEvent.Description = jiraBody?.Sprint?.Goal;
				jiraEvent.Color = 16771755;
			}

			if (!string.IsNullOrEmpty(jiraBody?.Sprint?.StartDate) && !string.IsNullOrEmpty(jiraBody?.Sprint?.EndDate))
			{
				DateTime startDate = DateTime.Parse(jiraBody.Sprint.StartDate);
				DateTime endDate = DateTime.Parse(jiraBody.Sprint.EndDate);

				jiraEvent.Author = $"{startDate.ToString("dd-MM-yyyy")} - {endDate.ToString("dd-MM-yyyy")}";
			}
		}

		private static void CreateIssue(JiraBody? jiraBody, ref JiraEvent jiraEvent)
		{
			jiraEvent.EventTypeLabel = $"{jiraBody?.Issue?.IssueField?.IssueType?.Name} Created";
			jiraEvent.Description = jiraBody?.Issue?.IssueField?.Description;
			jiraEvent.Author = jiraBody?.Issue?.IssueField?.Author?.DisplayName;
			jiraEvent.Color = 7667657;
		}

		private static void UpdateIssueStatus(JiraBody? jiraBody, ref JiraEvent jiraEvent)
		{
			if (jiraBody?.ChangeLog != null &&
				jiraBody.ChangeLog.ChangeLogItem?.Count > 0)
			{
				foreach (ChangeLogItem item in jiraBody.ChangeLog.ChangeLogItem)
				{
					if (item.Field == "status" && item.FieldType == "jira" && item.FieldId == "status")
					{
						jiraEvent.EventTypeLabel = "Status Updated";
						jiraEvent.Description = $"{item.FromString} moved to {item.ItemToString}";
						jiraEvent.Author = jiraBody?.Issue?.IssueField?.Author?.DisplayName;
						jiraEvent.Color = 7658746;
						return;
					}
				}
			}
		}

		private static void Comment(JiraBody? jiraBody, ref JiraEvent jiraEvent, string title)
		{
			if (string.IsNullOrEmpty(jiraBody?.Comment?.Body))
			{
				Console.WriteLine("Comment is missing!");
				return;
			}
			else
			{
				jiraEvent.EventTypeLabel = title;
				jiraEvent.Description = jiraBody.Comment.Body;
				jiraEvent.Author = jiraBody?.Comment?.UpdateAuthor?.DisplayName;
				jiraEvent.Color = 4540783;
			}
		}
	}
}
