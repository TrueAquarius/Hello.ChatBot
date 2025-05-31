using Azure;
using TrueAquarius.ConfigManager;
using TrueAquarius.ChatBot;
using Azure.AI.OpenAI;
using System.Net;
using OpenAI.Chat;
using System.Diagnostics;


namespace TrueAquarius.ChatBot;

public class Program
{
    private static AzureOpenAIClient azureClient;
    private static ChatClient chatClient;
    private static Configuration config = Configuration.Instance;

    // Chatverlauf speichern (SystemPrompt + User/Bot-Paare)
    private static readonly List<ChatMessage> chatHistory = new();

    // Define colors for different message types
    private const ConsoleColor UserColor = ConsoleColor.Cyan;
    private const ConsoleColor BotColor = ConsoleColor.Green;
    private const ConsoleColor SystemColor = ConsoleColor.Yellow;
    private const ConsoleColor ErrorColor = ConsoleColor.Red;
    private const ConsoleColor InfoColor = ConsoleColor.White;


    public static void Main(string[] args)
    {
        // Replace with your own OpenAI API key and model  
        string azureOpenAIAPIKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        string azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");

        if (string.IsNullOrEmpty(azureOpenAIAPIKey)
            || string.IsNullOrEmpty(azureOpenAIEndpoint))
        {
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine("Please set relevant environment variables.");
            Console.ResetColor();
            return;
        }


        azureClient = new(
            new Uri(azureOpenAIEndpoint),
            new AzureKeyCredential(azureOpenAIAPIKey));

        chatClient = azureClient.GetChatClient(config.LLM_DeploymentName);

        // Start the Chat with the user
        Console.ForegroundColor = SystemColor;
        Console.WriteLine("Welcome to the TrueAquarius ChatBot!");
        Console.Write("You are using Model ");
        Console.ForegroundColor = InfoColor;
        Console.WriteLine(config.LLM_DeploymentName);
        Console.ForegroundColor = SystemColor;
        Console.WriteLine("\n\n========== start chatting now ===========================");
        Console.WriteLine("Type '/help' for help. Type '/exit' or '/quit' to quit.\n");

        // SystemPrompt immer als erste Nachricht im Verlauf
        chatHistory.Clear();
        chatHistory.Add(new SystemChatMessage(config.SystemPrompt));

        while (true)
        {
            Console.ForegroundColor = UserColor;
            Console.Write("\n> ");
            string userPrompt = Console.ReadLine();
            Console.WriteLine("");

            if (string.IsNullOrEmpty(userPrompt))
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("Please enter a valid prompt.");
                continue;
            }

            CommandType commandType = HandleCommand(userPrompt);

            if (commandType == CommandType.EXIT) break;

            switch (commandType)
            {
                case CommandType.PROMPT:
                    // User prompt is valid, continue to process it
                    break;
                case CommandType.EMPTY:
                    // User entered an empty command, prompt again
                    continue;
                case CommandType.COMMAND:
                    // Command was handled, continue to next iteration
                    continue;

            }

            // Füge die Benutzernachricht zum Verlauf hinzu
            chatHistory.Add(new UserChatMessage(userPrompt));

            var response = chatClient.CompleteChatStreaming(chatHistory);

            Console.ForegroundColor = BotColor;
            var botResponseBuilder = new System.Text.StringBuilder();
            foreach (StreamingChatCompletionUpdate update in response)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    System.Console.Write(updatePart.Text);
                    botResponseBuilder.Append(updatePart.Text);
                }
            }
            System.Console.WriteLine("");

            // Füge die Bot-Antwort zum Verlauf hinzu
            chatHistory.Add(new AssistantChatMessage(botResponseBuilder.ToString()));

            CutChatHistory();
        }


        // Clean up and say bye  
        Console.ForegroundColor = SystemColor;
        Console.WriteLine("\nBye bye!\nIt was fun to talk to you.\n\n\n");
        Console.ResetColor();
    }



    private static void CutChatHistory()
    {
        // Verlauf ggf. kürzen (SystemPrompt bleibt immer erhalten)
        int maxHistory = config.HistoryLength * 2; // User+Bot pro Runde
        if (chatHistory.Count > maxHistory + 1)
        {
            // Entferne die ältesten User+Bot-Paare, SystemPrompt bleibt
            chatHistory.RemoveRange(1, chatHistory.Count - (maxHistory + 1));
        }
    }



    /// <summary>
    /// Handles the command input by the user.
    /// </summary>
    /// <param name="commandLine"></param>
    /// <returns>true: The command required the program to terminate.</returns>
    private static CommandType HandleCommand(string commandLine)
    {
        commandLine = commandLine?.Trim() ?? string.Empty;

        if (!commandLine.StartsWith("/"))
        {
            // If the command does not start with '/', treat it as a user prompt
            return CommandType.PROMPT;
        }

        commandLine = commandLine.Substring(1).Trim(); // Remove the leading '/' and trim whitespace

        if (string.IsNullOrEmpty(commandLine))
        {
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine("Command cannot be empty. Type '/help' for help. Type '/exit' or '/quit' to quit.");
            return CommandType.EMPTY;
        }

        string cmd = commandLine.Split(' ')[0]; // Get the command name (first word)

        switch (cmd)
        {
            case "exit":
            case "quit":
                return CommandType.EXIT;
            case "help":
                return HelpCommand(CommandBody(commandLine));
            case "model":
                return ModelCommand(CommandBody(commandLine));
            case "clear":
                chatHistory.RemoveRange(1, chatHistory.Count - 1); // Keep only the SystemPrompt
                return CommandType.COMMAND;
            case "history":
                return HistoryCommand(CommandBody(commandLine));
            default:
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("Unknown command. Type 'exit' or 'quit' to break.");
                return CommandType.EMPTY;
        }
    }

    private static string CommandBody(string command)
    {
        if (string.IsNullOrEmpty(command) || !command.Contains(' '))
        {
            return string.Empty;
        }
        int spaceIndex = command.IndexOf(' ');
        return command.Substring(spaceIndex + 1).Trim();
    }


    private static CommandType HelpCommand(string commandLine)
    {
        Console.ForegroundColor = InfoColor;
        Console.WriteLine("Available commands:");
        Console.WriteLine("/clear            -   Clear chat history");
        Console.WriteLine("/help             -   Show this help message");
        Console.WriteLine("/history          -   Show history length");
        Console.WriteLine("/history [length] -   Set history length");
        Console.WriteLine("/model            -   Show the current model");
        Console.WriteLine("/model [model]    -   Set the current model");
        Console.WriteLine("/exit or /quit    -   Exit the chat");
        Console.WriteLine("Type your question or prompt to start chatting with the AI.");

        return CommandType.COMMAND;
    }

    private static CommandType ModelCommand(string commandLine)
    {
        if (string.IsNullOrEmpty(commandLine))
        {
            Console.ForegroundColor = InfoColor;
            Console.WriteLine("Current Model: " + config.LLM_DeploymentName);
            return CommandType.COMMAND;
        }

        string[] parts = commandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        chatClient = azureClient.GetChatClient(config.LLM_DeploymentName);

        config.LLM_DeploymentName = parts[0];
        config.Save();

        Console.ForegroundColor = InfoColor;
        Console.WriteLine("Model changed to: " + config.LLM_DeploymentName);

        return CommandType.COMMAND;
    }

    private static CommandType HistoryCommand(string commandLine)
    {
        if (string.IsNullOrEmpty(commandLine))
        {
            Console.ForegroundColor = InfoColor;
            Console.WriteLine("Current history length: " + chatHistory.Count / 2);
            Console.WriteLine("Max history length:     " + config.HistoryLength);
            return CommandType.COMMAND;
        }

        string[] parts = commandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        bool success = int.TryParse(parts[0], out int newHistoryLength);

        if (!success || newHistoryLength < 0)
        {
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine("Invalid history length. Please enter a valid number greater than zero.");
            return CommandType.COMMAND;
        }

        config.HistoryLength = newHistoryLength;
        config.Save();
        CutChatHistory();

        Console.ForegroundColor = InfoColor;
        Console.WriteLine("History length set to: " + config.HistoryLength);

        return CommandType.COMMAND;
    }
}



