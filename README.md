# Hello.ChatBot

This is a simple Chatbot using **Azure OpenAI**. It runs in your Console.

## Features
- Chat with your Azure OpenAI instance from your Console
- Switch between different models (more precisely: switch between different deployments) with the `/model` command.
- Basic chat history


## Requirements

You need to have an Azure OpenAI account and create at least one deployment. Set the api key and enpoint in your environment variables as explained below.

## Installation

You need to set the following environment variables:

In the Console:
```
setx AZURE_OPENAI_API_KEY "[your Azure OpenAI key]"
setx AZURE_OPENAI_ENDPOINT "[your Azure OpenAI endpoint URL]"
```

In PowerShell:
```
[System.Environment]::SetEnvironmentVariable("AZURE_OPENAI_API_KEY", "[your Azure OpenAI key]", "User")
[System.Environment]::SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", "[your Azure OpenAI endpoint URL]", "User")
```

The default deployment name right after setup is 'gpt-4o'. If your deployment name in Azure is different, you can change it using the `/model` command:

```
/model [your deployment name]
```

enjoy it!!!


