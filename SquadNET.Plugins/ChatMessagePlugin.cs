using System;
using System.Reflection;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events;
using SquadNET.Core.Squad.Models;
using SquadNET.Plugins.Abstractions;

namespace SquadNET.Plugins
{
    public class ChatMessagePlugin : Plugin
    {
        public override string Name => "ChatMessagePlugin";

        /// <summary>
        /// Sobrescribimos OnEventRaised para procesar CHAT_MESSAGE.
        /// </summary>
        public override void OnEventRaised(string eventName, IEventData eventData)
        {
            if (eventName == SquadEventType.ChatMessage.ToString())
            {
                if (eventData is ChatMessageEventModel chat)
                {
                    string message = chat.ToString();
                    WriteToLogFile(message);
                }
            }
        }
        private void WriteToLogFile(string message)
        {
            try
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string logFilePath = Path.Combine(assemblyPath, "ChatMessages.log");

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
