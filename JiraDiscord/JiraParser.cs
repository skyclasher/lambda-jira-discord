using JiraDiscord.Constants;
using JiraDiscord.Models.Jira;

namespace JiraDiscord
{
	public static class JiraParser
	{
		public static JiraEvent Parse(JiraBody jiraBody)
		{
			if (string.IsNullOrEmpty(jiraBody.WebhookEvent))
			{
				throw new ArgumentNullException("Webhook Event Missing!");
			}

			if (jiraBody?.Issue?.IssueUrl == null || jiraBody.Issue.Key == null)
			{
				throw new ArgumentNullException("Issue url or key is missing!");
			}
			else
			{
				JiraEvent jiraEvent = new JiraEvent()
				{
					Key = jiraBody.Issue.Key,
					Url = ConstructUrl(jiraBody.Issue.IssueUrl, jiraBody.Issue.Key),
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
					Comment(jiraBody, ref jiraEvent, "Comment Created");
				}
				else
				{
					throw new Exception("Event is not supported");
				}
				return jiraEvent;
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
					}
				}
			}
		}

		private static void Comment(JiraBody? jiraBody, ref JiraEvent jiraEvent, string title)
		{
			if (string.IsNullOrEmpty(jiraBody?.Comment?.Body))
			{
				throw new ArgumentNullException("Comment is missing!");
			}
			else
			{
				jiraEvent.EventTypeLabel = title;
				jiraEvent.Description = jiraBody.Comment.Body;
				jiraEvent.Author = jiraBody?.Comment?.UpdateAuthor?.DisplayName;
				jiraEvent.Color = 4540783;
			}
		}

		private static string ConstructUrl(string url, string key)
		{
			Uri uri = new Uri(url);
			return $"https://{uri.Host}/browse/{key}";
		}
	}
}
