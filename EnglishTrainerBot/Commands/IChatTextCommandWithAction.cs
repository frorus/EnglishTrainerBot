namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Расширение IChatTextCommand с действием
    /// </summary>
    public interface IChatTextCommandWithAction : IChatTextCommand
    {
        bool DoAction(Conversation chat);
    }
}
