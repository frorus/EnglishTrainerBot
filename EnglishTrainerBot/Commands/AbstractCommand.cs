namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Абстрактная команда чата, содержит конкретную реализацию текста команды (того, что отправляет пользователь) и проверки, является ли сообщение пользователя командой или нет. 
    /// </summary>
    public abstract class AbstractCommand : IChatCommand
    {
        public string CommandText;

        public virtual bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
