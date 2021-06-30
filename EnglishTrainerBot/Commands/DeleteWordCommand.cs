namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Команда удаления слова из словаря
    /// </summary>
    public class DeleteWordCommand : ChatTextCommandOption, IChatTextCommandWithAction
    {
        public DeleteWordCommand()
        {
            CommandText = "/deleteword";
        }

        /// <summary>
        /// Получает сообщение чата (последнее). Забирает из него текст слова, то есть убираем наименование команды и пробел после него.
        /// Проверяет по словарю, есть ли это слово. Если есть, убирает. Если нет, возвращает ошибку — слова в словаре нет. 
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool DoAction(Conversation chat)
        {
            var message = chat.GetLastMessage();

            var text = ClearMessageFromCommand(message);

            if (chat.Dictionary.ContainsKey(text))
            {
                chat.Dictionary.Remove(text);
                return true;
            }

            return false; 
        }

        public string ReturnText()
        { 
            return "Слово успешно удалено!";
        }
    }
}