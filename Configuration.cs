using TrueAquarius.ConfigManager;

namespace TrueAquarius.ChatBot;

/// <summary>
/// Represents the configuration settings for the chat bot.
/// This is loaded on application startup and can be modified by the user.
/// The default values stated here are overwritten by the values in the config file.
/// The config file is typically located [user]/AppData/Roaming/TrueAquarius/ChatBot/config.json.
/// </summary>
internal class Configuration : ConfigManager<Configuration>
{
    public string DeploymentName { get; set; } = "gpt-4o";
    public int HistoryLength { get; set; } = 5;
    public float Temperature { get; set; } = 0.7f;
    public int MaxOutputTokenCount { get; set; } = 1000;
    public string SystemPrompt { get; set; } = "You are a helpful assistant. Please answer the user's questions to the best of your ability.";
}
