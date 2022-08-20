# JIRA Discord AWS Lambda Webhook
A simple AWS lambda function that takes a JIRA webhook payload, creates a Discord compatible webhook payload and executes the appropriate Discord webhook. This repository is inspired by aucerna/jira-discord-lambda that was written with scala. This repository however is code in dotnet. Some of the functionality has been change to accomadate my specification.

Setup instruction:
1) Create a AWS Lambda function. Choose .Net 6(C#/PowerShell) as the runtime and upload the published code.
2) Change the handler to JiraDiscord::JiraDiscord.Program::Handler 
3) Create a HTTP API in AWS API Gateway
4) Add integration with the lambda function. Choose Version 2.0
5) Define your prefered route.
6) Create a webhook in Discord, take note the url will contain discord id and token.
7) Create JIRA webhook from setting->system->WebHooks.
8) Fill in the URL with this format: (https://[Api_Url]/[Route]?proxy=[Discord_Id]/[Discord_Token])
9) Save and test the webhook.

This diagram below shows how the flow work. Credit to aucerna. See his blog post to see more depth in technical view.
![](architecture.png)

### Resources
https://github.com/aucerna/jira-discord-lambda
https://discordapp.com/developers/docs/resources/webhook#execute-webhook