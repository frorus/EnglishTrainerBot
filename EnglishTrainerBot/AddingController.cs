using System.Collections.Generic;
using EnglishTrainerBot.EnglishTrainer.Model;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Контролёр состояния чата
    /// </summary>
    public class AddingController
    {
        private Dictionary<long, AddingState> chatAdding;

        public AddingController()
        {
            chatAdding = new Dictionary<long, AddingState>();
        }

        /// <summary>
        /// Метод добавляет чат в словарь со статусом «русский», то есть требует ввод слова на русском
        /// </summary>
        /// <param name="chat"></param>
        public void AddFirstState(Conversation chat)
        {
            chatAdding.Add(chat.GetId(), AddingState.Russian);
        }

        /// <summary>
        /// Метод переходит по статусам, и когда дойдет до последнего, то говорит чату «процесс добавления слова окончен» и убирает чат из словаря
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chat"></param>
        public void NextStage(string message, Conversation chat)
        {
            var currentstate = chatAdding[chat.GetId()];
            chatAdding[chat.GetId()] = currentstate + 1;

            if (chatAdding[chat.GetId()] == AddingState.Finish)
            {
                chat.IsAddingInProcess = false;
                chatAdding.Remove(chat.GetId());
            }
        }

        /// <summary>
        /// Получение статуса тренировки
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public AddingState GetStage(Conversation chat)
        {
            return chatAdding[chat.GetId()];
        }
    }
}