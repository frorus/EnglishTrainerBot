using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using EnglishTrainerBot.EnglishTrainer.Model;

namespace EnglishTrainerBot
{
    /// <summary>
    /// Класс представляет собой объект чата
    /// </summary>
    public class Conversation
    {
        public bool IsAddingInProcess;

        public bool IsTraningInProcess;

        public Dictionary<string, Word> Dictionary;

        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            Dictionary = new Dictionary<string, Word>();
        }

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public long GetId() => telegramChat.Id;

        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;

        public string GetWordList()
        {
            var sb = new StringBuilder();
            string text = string.Empty;

            foreach (KeyValuePair<string, Word> keyValue in Dictionary)
            {
                text = sb.Append($"{keyValue.Value.Russian} - {keyValue.Value.English}\n").ToString();
            }

            return text;
        }

        public string GetTrainingWord(TrainingType type)
        {
            var rand = new Random();
            var item = rand.Next(0, Dictionary.Count);

            var randomword = Dictionary.Values.AsEnumerable().ElementAt(item);

            var text = string.Empty;

            switch (type)
            {
                case TrainingType.EngToRus:
                    text = randomword.English;
                    break;

                case TrainingType.RusToEng:
                    text = randomword.Russian;
                    break;
            }

            return text;
        }

        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;

            var result = false;

            switch (type)
            {
                case TrainingType.EngToRus:

                    control = Dictionary.Values.FirstOrDefault(x => x.English == word);

                    result = control.Russian == answer;

                    break;

                case TrainingType.RusToEng:
                    control = Dictionary[word];

                    result = control.English == answer;

                    break;
            }

            return result; 
        }
    }
}
