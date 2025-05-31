using TrueAquarius.ConfigManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueAquarius.ChatBot
{
    internal class Configuration : ConfigManager<Configuration>
    {
        public string DeploymentName { get; set; } = "gpt-4.1";
        public int MaxTokens { get; set; } = 1000;
        public int HistoryLength { get; set; } = 5;
        public float Temperature { get; set; } = 0.7f;
        public int MaxOutputTokenCount { get; set; } = 10000;
        public string SystemPrompt { get; set; } = "You are a helpful assistant. Please answer the user's questions to the best of your ability.";
    }
}
