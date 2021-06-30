using System.Collections.Generic;
using Telegram.Bot;

namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Команда вывода сохраненных слов
    /// </summary>
    public class DictionaryCommand : AbstractCommand, IChatTextCommand
    {
        private ITelegramBotClient botClient;
        private Dictionary<long, Conversation> userChat;

        public DictionaryCommand(ITelegramBotClient botClient)
        {
            CommandText = "/dictionary";

            this.botClient = botClient;
            userChat = new Dictionary<long, Conversation>();
        }

        public string ReturnText()
        {
            return "Сохранённые слова:\n";
        }


        /// <summary>
        /// Метод добавляет чат, полуает его id и возвращает словарь сохраненных слов
        /// </summary>
        /// <param name="chat"></param>
        public async void ShowDictionary(Conversation chat)
        {
            userChat.Add(chat.GetId(), chat);

            var id = chat.GetId();

            var dictionary = userChat[chat.GetId()];

            var text = dictionary.GetWordList();

            if (userChat.ContainsKey(id))
            {
                userChat.Remove(id);
            }

            await botClient.SendTextMessageAsync(id, text);
        }
    }
}