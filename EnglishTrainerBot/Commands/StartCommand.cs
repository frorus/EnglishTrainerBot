namespace EnglishTrainerBot.Commands
{
    /// <summary>
    /// Стартовая команда
    /// </summary>
    public class StartCommand : AbstractCommand, IChatTextCommand
    {
        public StartCommand()
        {
            CommandText = "/start";
        }

        public string ReturnText()
        {
            return "Список команд:\n" +
                " /addword - Добавление слова в словарь\n" +
                " /deleteword - Удаление слова из словаря. Пример ввода: /deleteword слово\n" +
                " /dictionary - Отобразить все сохраненные слова\n" +
                " /training - Начать тренировку\n" +
                " /stop - Завершить тренировку";
        }
    }
}
