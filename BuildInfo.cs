namespace TrueAquarius.ChatBot;

/// <summary>
/// Represents the build information for the chat bot application.
/// BuildDate and BuildNumber are automatically updated by the build process.
/// </summary>
public static class BuildInfo
{
    /// <summary>
    /// The date and time when the build was created.
    /// Automatically updated by the build process.
    /// </summary>
    public const string BuildDate = "01.06.2025 12:00 +02:00"; // DO NOT CHANGE THIS MANUALLY! This is automatically updated by the build process.

    /// <summary>
    /// The build number of the application.
    /// Automatically updated by the build process.
    /// </summary>
    public const int BuildNumber = 437; // DO NOT CHANGE THIS MANUALLY! This is automatically updated by the build process.

    /// <summary>
    /// Version number of the application.
    /// Must be updates manually when a new version is released.
    /// </summary>
    public const string Version = "V1.0.6";

    /// <summary>
    /// Combines the version and build number into a single string for display purposes.
    /// </summary>
    public static string All { get { return Version + " (Build " + BuildNumber + ")"; } }
}
