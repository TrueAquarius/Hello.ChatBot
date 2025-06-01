# Hello.ChatBot  

This is a simple Chatbot using **Azure OpenAI**. It runs in your Console.

Like all my "Hello"-projects, this is more a Hello World version of a chat bot. however, it has evolved into a quite useful tool for those peole who want to user their own instance of OpenAI.

## Features  
- Chat with your Azure OpenAI instance from your Console  
- Switch between different models (more precisely: switch between different deployments) with the `/model` command.  
- Chat history; length of history can be set with `\history` command.  
- Get help with `\help` command  


## Screenshot  
- The following screenshot shows the start of a chat. The user asks a question; the bot answers.
- Then the user inquires which model is being used.
- The user changes the model
- the user asks another question

![Screenshot](./_Documents/Screenshot.png)  


## Requirements

You need to have an Azure OpenAI account and create at least one deployment. Set the api key and endpoint in your environment variables as explained below.

## Installation

You need to set the following environment variables:

In the Console:
```powershell
setx AZURE_OPENAI_API_KEY "[your Azure OpenAI key]"
setx AZURE_OPENAI_ENDPOINT "[your Azure OpenAI endpoint URL]"
```

In PowerShell:
```powershell
[System.Environment]::SetEnvironmentVariable("AZURE_OPENAI_API_KEY", "[your Azure OpenAI key]", "User")
[System.Environment]::SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", "[your Azure OpenAI endpoint URL]", "User")
```

The default deployment name right after setup is 'gpt-4o'. If your deployment name in Azure is different, you can change it using the `/model` command:

```
/model [your deployment name]
```

## How to use it
Type any prompt and get an answer from Azure OpenAI.

Change settings with `/`-commands. Type `/help` to get list of commands.

**Enjoy it!!!**


## How to make new release
Building a release is supported by an GitHub Action ([release.yml](.github/workflows/release.yml)), which gets triggered when a `git push` contains a tag which starts with the letter '`v`' like '`v1.0.0`'.

Extract form [release.yml](.github/workflows/release.yml):
```yml
on:
  push:
    tags:
      - 'v*'
```

In Visual Studio 2022, open a new terminal. Create a new tag with the release number:
```powershell
git tag -a v1.0.0 -m "Release version 1.0.0"
```
Make a local commit. Then push from terminal with tag number:
```powershell
git push origin v1.0.0
```