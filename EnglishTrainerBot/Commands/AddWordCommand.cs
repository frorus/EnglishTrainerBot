using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using EnglishTrainerBot.EnglishTrainer.Model;

namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Команда добавления слова в словарь
    /// </summary>
    public class AddWordCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;

        private Dictionary<long, Word> buffer;

        public AddWordCommand(ITelegramBotClient botClient)
        {
            CommandText = "/addword";

            this.botClient = botClient;

            buffer = new Dictionary<long, Word>();
        }

        /// <summary>
        /// При первом вызове команды AddWordCommand метод запрашивает русский перевод слова, а также добавляет в словарь чат и текущее слово, которое заполняется им
        /// </summary>
        /// <param name="chat"></param>
        public async void StartProcessAsync(Conversation chat)
        {
            buffer.Add(chat.GetId(), new Word());

            var text = "Введите русское значение слова";

            await SendCommandText(text, chat.GetId());
        }

        /// <summary>
        /// Добавление слова в словарь
        /// </summary>
        /// <param name="addingState"></param>
        /// <param name="chat"></param>
        /// <param name="message"></param>
        public async void DoForStageAsync(AddingState addingState, Conversation chat, string message)
        {
            var word = buffer[chat.GetId()];
            var text = string.Empty;

            switch (addingState)
            {
                case AddingState.Russian:
                    word.Russian = message;

                    text = "Введите английское значение слова";
                    break;

                case AddingState.English:
                    word.English = message;

                    text = "Введите тематику";
                    break;

                case AddingState.Theme:
                    word.Theme = message;

                    text = "Успешно! Слово " + word.English + " добавлено в словарь. ";

                    chat.Dictionary.Add(word.Russian, word);

                    buffer.Remove(chat.GetId());
                    break;
            }

            await SendCommandText(text, chat.GetId());
        }

        /// <summary>
        /// Отправка сообщения в чат
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}