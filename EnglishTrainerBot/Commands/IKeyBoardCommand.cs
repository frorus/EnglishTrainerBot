using Telegram.Bot.Types.ReplyMarkups;

namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Команда, вызываемая через кнопки
    /// </summary>
    public interface IKeyBoardCommand
    {
        InlineKeyboardMarkup ReturnKeyBoard();

        void AddCallBack(Conversation chat);

        string InformationalMessage();
    }
}