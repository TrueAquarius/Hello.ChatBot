using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Azure;
using Microsoft.SemanticKernel.ChatCompletion;


// Some of the NuGet Packages are still in pre-release or alpha state. Need to suppress compiler warnings
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010


// Replace with your own OpenAI API key and model  
string azureOpenAIAPIKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
string azureOpenAIDeploymentName = "gpt-4.1";  // Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT") ?? "gpt-4o";
string azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");

if (string.IsNullOrEmpty(azureOpenAIAPIKey)
    || string.IsNullOrEmpty(azureOpenAIDeploymentName)
    || string.IsNullOrEmpty(azureOpenAIEndpoint))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Please set relevant environment variables.");
    Console.ResetColor();
    return;
}


// Configure and build the kernel
Console.WriteLine($"Configuring and building the kernel");
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(azureOpenAIDeploymentName, azureOpenAIEndpoint, azureOpenAIAPIKey);
var kernel = builder.Build();






// Start the Chat with the user
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\n\n========== start chatting ==========");
Console.WriteLine("Type 'exit' or 'quit' to break.\n\n");
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
OpenAIPromptExecutionSettings settings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 1000,
};

var history = new ChatHistory();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("> ");
    string input = Console.ReadLine();

    if (input?.Trim().ToLower() is "exit" or "quit") break;

    
    // Combine retrieved content into a system message or context
    string promptWithContext = $"{input}";

    history.AddUserMessage(promptWithContext);

    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: settings,
        kernel: kernel
    );

    history.AddMessage(result.Role, result.Content ?? "");
    while (history.Count > 20)
    {
        history.RemoveAt(0);
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nAI Agent:\n{result.Content}\n");
}


// Clean up and say bye  
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\nBye bye!\nIt was fun to talk to you.\n\n\n");
Console.ResetColor();