using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Класс управляет отправкой сообщений и логикой добавления чатов
    /// </summary>
    public class BotMessageLogic
    {
        private Messenger messanger;

        private Dictionary<long, Conversation> chatList;

        private ITelegramBotClient botClient;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            messanger = new Messenger(botClient);
            chatList = new Dictionary<long, Conversation>();
        }

        /// <summary>
        /// Обработка полученных сообщений
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;

            if (!chatList.ContainsKey(id))
            {
                var newchat = new Conversation(e.Message.Chat);

                chatList.Add(id, newchat);
            }

            var chat = chatList[id];

            chat.AddMessage(e.Message);

            await SendMessage(chat);
        }

        private async Task SendMessage(Conversation chat)
        {
            await messanger.MakeAnswer(chat);
        }
    }
}