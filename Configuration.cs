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
        public string LLM_DeploymentName { get; set; } = "gpt-4.1";
        public int MaxTokens { get; set; } = 1000;
        public int HistoryLength { get; set; } = 5;
        public string SystemPrompt { get; set; } = "You are a helpful assistant. Please answer the user's questions to the best of your ability.";
    }
}
