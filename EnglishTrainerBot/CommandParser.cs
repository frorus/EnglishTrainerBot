using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using EnglishTrainerBot.Commands;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Анализатор команд
    /// </summary>
    public class CommandParser
    {
        private List<IChatCommand> command;

        private AddingController addingController;

        public CommandParser()
        {
            command = new List<IChatCommand>();
            addingController = new AddingController();
        }

        public void AddCommand(IChatCommand chatCommand)
        {
            command.Add(chatCommand);
        }

        public bool IsMessageCommand(string message)
        {
           return command.Exists(x => x.CheckMessage(message));
        }

        public bool IsTextCommand(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public bool IsButtonCommand(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        public bool IsDisplayDictionary(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));
            return command is DictionaryCommand;
        }

        public string GetMessageText(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            if (command is IChatTextCommandWithAction)
            {
                if (!(command as IChatTextCommandWithAction).DoAction(chat))
                {
                    return "Ошибка выполнения команды!";
                }
            }

            return command.ReturnText();
        }

        public string GetInformationalMessage(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.InformationalMessage();
        }

        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command.AddCallBack(chat);
        }

        public bool IsAddingCommand(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));

            return command is AddWordCommand;
        }

        public void StartAddingWord(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as AddWordCommand;

            addingController.AddFirstState(chat);
            command.StartProcessAsync(chat);
        }

        public void NextStage(string message, Conversation chat)
        {
            var command = this.command.Find(x => x is AddWordCommand) as AddWordCommand;

            command.DoForStageAsync(addingController.GetStage(chat), chat, message);

            addingController.NextStage(message, chat);
        }

        public void ContinueTraining(string message, Conversation chat)
        {
            var command = this.command.Find(x => x is TrainingCommand) as TrainingCommand;

            command.NextStepAsync(chat, message);
        }

        public void DisplayDictionary(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as DictionaryCommand;
            command.ShowDictionary(chat);
        }
    }
}
