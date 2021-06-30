using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using EnglishTrainerBot.EnglishTrainer.Model;

namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Команда запуска тренировки
    /// </summary>
    public class TrainingCommand : AbstractCommand, IKeyBoardCommand
    {
        private ITelegramBotClient botClient;

        private Dictionary<long, TrainingType> training;

        private Dictionary<long, Conversation> trainingChats;

        private Dictionary<long, string> activeWord;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";

            this.botClient = botClient;

            training = new Dictionary<long, TrainingType>();
            trainingChats = new Dictionary<long, Conversation>();
            activeWord = new Dictionary<long, string>();
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "С русского на английский",
                    CallbackData = "rustoeng"
                },

                new InlineKeyboardButton
                {
                    Text = "С английского на русский",
                    CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }

        /// <summary>
        /// Чат записывается в «лист ожидания»
        /// </summary>
        /// <param name="chat"></param>
        public void AddCallBack(Conversation chat)
        {
            trainingChats.Add(chat.GetId(), chat);

            this.botClient.OnCallbackQuery -= Bot_Callback;
            this.botClient.OnCallbackQuery += Bot_Callback;
        }

        /// <summary>
        /// Выбирает слово из словаря для тренировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = string.Empty;

            var id = e.CallbackQuery.Message.Chat.Id;

            var chat = trainingChats[id];

            switch (e.CallbackQuery.Data)
            {
                case "rustoeng":
                    training.Add(id, TrainingType.RusToEng);

                    text = chat.GetTrainingWord(TrainingType.RusToEng);

                    break;
                case "engtorus":
                    training.Add(id, TrainingType.EngToRus);

                    text = chat.GetTrainingWord(TrainingType.EngToRus);
                    break;
                default:
                    break;
            }

            chat.IsTraningInProcess = true;
            activeWord.Add(id, text);

            if (trainingChats.ContainsKey(id))
            {
                trainingChats.Remove(id);
            }

            await botClient.SendTextMessageAsync(id, text);
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }

        public async void NextStepAsync(Conversation chat, string message)
        {
            var type = training[chat.GetId()];
            var word = activeWord[chat.GetId()];

            var check = chat.CheckWord(type, word, message);

            var text = string.Empty;

            if (check)
            {
                text = "Правильно!";
            }
            else
            {
                text = "Неправильно!";
            }

            text = text + " Следующее слово: ";

            var newword = chat.GetTrainingWord(type);

            text = text + newword;

            activeWord[chat.GetId()] = newword;

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
    }
}
