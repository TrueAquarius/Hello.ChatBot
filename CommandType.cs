namespace TrueAquarius.ChatBot;

/// <summary>
/// Represents the type of command being processed by the chat bot.
/// </summary>
internal enum CommandType
{
    /// <summary>
    /// Represents a command to exit the chat bot.
    /// </summary>
    EXIT,

    /// <summary>
    /// Represents a general command issued to the chat bot.
    /// </summary>
    COMMAND,

    /// <summary>
    /// Represents a prompt or query for the chat bot to respond to.
    /// </summary>
    PROMPT,

    /// <summary>
    /// Represents an empty or unrecognized input.
    /// </summary>
    EMPTY,
}
