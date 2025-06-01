namespace TrueAquarius.ChatBot;

public static class BuildInfo
{
    public const string BuildDate = "01.06.2025 10:28 +02:00";
    public const int BuildNumber = 435;
    public const string Version = "V1.0.4";

    public static string All { get { return Version + " (Build " + BuildNumber + ")"; } }
}
