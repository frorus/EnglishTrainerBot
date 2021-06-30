using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Класс отвечает за работу клиента бота на верхнем уровне
    /// </summary>
    public class BotWorker
    {
        private ITelegramBotClient botClient;

        private BotMessageLogic logic;

        /// <summary>
        /// Метод создает клиент бота
        /// </summary>
        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }

        /// <summary>
        /// Метод устанавливает событие на отправку сообщений и начинает ожидание этих сообщений
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        /// <summary>
        /// Завершает процесс Start
        /// </summary>
        public void Stop()
        {
            botClient.StopReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await logic.Response(e);
            }
        }
    }
}
