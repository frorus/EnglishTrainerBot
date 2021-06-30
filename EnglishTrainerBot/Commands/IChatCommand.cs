namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Базовая команда чата, все команды реализуют данный интерфейс.
    /// </summary>
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
