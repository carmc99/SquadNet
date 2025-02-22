using System;
using SquadNET.Plugins.Abstractions;
using SquadNET.Plugins.Abstractions.Squad;

namespace SquadNET.Plugins
{
    public class ChatMessagePlugin : Plugin
    {
        public override string Name => "ChatMessagePlugin";

        /// <summary>
        /// Sobrescribimos OnEventRaised para procesar CHAT_MESSAGE.
        /// </summary>
        public override void OnEventRaised(string eventName, object eventData)
        {
            if (eventName == LogEventType.CHAT_MESSAGE.ToString())
            {
                // Convertimos eventData al tipo que esperamos (ChatMessageInfo u otro)
                if (eventData is ChatMessageInfo chat)
                {
                    // Aquí tu lógica: por ejemplo, imprimir mensaje en consola, guardar en DB, etc.
                    Console.WriteLine($"[{Name}] Nuevo Chat: {chat.Name} => {chat.Message}");
                }
            }
        }
    }
}
