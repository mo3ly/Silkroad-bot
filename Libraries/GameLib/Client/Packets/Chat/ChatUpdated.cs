using System;
using SilkroadSecurityApi;

namespace SilkroadInformationAPI.Client.Packets.Chat
{
    public class ChatUpdated
    {
        public static event Action<Information.Chat.ChatMessage> OnChatReceive;
        public static void ParseServer(Packet p)
        {
            uint uniqueID = 0;
            string name = "";

            var chatType = (ChatType)p.ReadUInt8();
            if (chatType == ChatType.All ||
                chatType == ChatType.AllGM ||
                chatType == ChatType.NPC)
            {
                uniqueID = p.ReadUInt32();

            }
            else if (chatType == ChatType.PM ||
                     chatType == ChatType.Party ||
                     chatType == ChatType.Guild ||
                     chatType == ChatType.Global ||
                     chatType == ChatType.Stall ||
                     chatType == ChatType.Union ||
                     chatType == ChatType.Accademy)
            {
                name = p.ReadAscii();
            }

            string message = p.ReadAscii();

            /*
            if (name == "")
                name = Actions.Mapping.GetCharNameFromUID(uniqueID);

            OnChatReceive?.Invoke(new Information.Chat.ChatMessage(message, name, uniqueID, chatType));

            if (Client.Chat.ContainsKey(chatType))
                Client.Chat[chatType].Add(new Information.Chat.ChatMessage(message, name, uniqueID, chatType));
            else
                Client.Chat.Add(chatType, new List<Information.Chat.ChatMessage>() { new Information.Chat.ChatMessage(message, name, uniqueID, chatType) });

            if (Client.Chat.ContainsKey(ChatType.All) && chatType != ChatType.All)
                Client.Chat[ChatType.All].Add(new Information.Chat.ChatMessage(message, name, uniqueID, chatType));
            else if (!Client.Chat.ContainsKey(ChatType.All) && chatType != ChatType.All)
                Client.Chat.Add(ChatType.All, new List<Information.Chat.ChatMessage>() { new Information.Chat.ChatMessage(message, name, uniqueID, chatType) });
            */
        }

        public static void ParseClient(Packet p)
        {
            p.ReadUInt8();
            p.ReadUInt8();
            string message = p.ReadAscii();
            //Contollers.GameBot.ChatCommands.ChatFilter(message);
        }
    }
}
