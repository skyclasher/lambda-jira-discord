# JIRA Discord AWS Lambda Webhook
A simple AWS lambda function that takes a JIRA webhook payload, creates a Discord-compatible webhook payload, and executes the appropriate Discord webhook. This repository is inspired by aucerna/jira-discord-lambda which was written with scala. This repository however is code in dotnet. Some of the functionality has been changed to accommodate my specification.

Setup instruction:
01) Create an AWS Lambda function. Choose .Net 6(C#/PowerShell) as the runtime and upload the published code.
02) Define the environment variable in lambda:-
    - discord_id		- Discord webhook id
    - discord_token	- Discord webhook token
    - jira_api_key		- Jira Rest API token key.
    - jira_email		- Jira email of the generated token.
03) Change the handler to JiraDiscord::JiraDiscord.Program::Handler
04) Add DynamoDb table named "jira_user_mapping" with hash key of "jira_account_id" string. DynamoDb is used to store user mapping of jira account id and the display name.
05) Add the DynamoDb accsess to the lambda role.
06) Create an HTTP API in AWS API Gateway
07) Add integration with the lambda function. Choose Version 2.0
08) Define your preferred route.
09) Create a webhook in Discord, take note the URL will contain the discord id and token.
10) Create JIRA webhook from setting->system->WebHooks.
11) Fill in the URL with this format: (https://[Api_Url]/[Route]?projectKey=${project.key})
12) Save and test the webhook.

The diagram below shows how the flow work. Credit to aucerna. See his blog post to see more depth in technical view. 

![](architecture.png)

### Resources
https://github.com/aucerna/jira-discord-lambda
https://discordapp.com/developers/docs/resources/webhook#execute-webhook
