namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Расширение AbstractCommand
    /// </summary>
    public abstract class ChatTextCommandOption : AbstractCommand
    {
        public override bool CheckMessage(string message)
        {
            return message.StartsWith(CommandText);
        }

        /// <summary>
        /// Отделяет команду от слова
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ClearMessageFromCommand(string message)
        {
            return message.Substring(CommandText.Length + 1);
        }
    }
}