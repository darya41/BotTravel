using BotTravel;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    private static InlineKeyboardMarkup inlineKeyboard;
    private static ReplyKeyboardMarkup keyboard;
    private static WorkWithJson workWithJson = new WorkWithJson();
    private static List<Advice> advises;

    static async Task Main(string[] args)
    {
        advises = workWithJson.ReadJsonFile<Advice>("Advice.json");
        using var cts = new CancellationTokenSource();
        var botToken = "7010950024:AAFqwJmawUifogh6ZjpH3Hxjr9NSPGM_vcs"; // Замените на свой токен
        var client = new TelegramBotClient(botToken);
        await client.DeleteWebhookAsync();

        // Создаем кнопки встроенного меню
        var button1 = new KeyboardButton("Викторина");
        var button2 = new KeyboardButton("Интерактивный опрос");
        var button3 = new KeyboardButton("Получить совет");
        var button4 = new KeyboardButton("Узнать факт");
        var button5 = new KeyboardButton("Информация о интересных местах города");

        keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { button1, button2 }
            , new[] { button3, button4 }
            , new[] { button5 }
        })
        {
            ResizeKeyboard = true
        };

        client.StartReceiving(Update, Error, new Telegram.Bot.Polling.ReceiverOptions(), cts.Token);

        var me = await client.GetMeAsync();
        Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
        Console.ReadLine();
        cts.Cancel(); // stop the bot
    }

    async static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        Console.WriteLine($"Error: {exception.Message}");
    }

    async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Type == UpdateType.Message && update.Message != null && update.Message.Type == MessageType.Text)
        {
            var message = update.Message;
            
            var chatId = message.Chat.Id;

            if (message.Text.ToLower() == "/start")
            {
                Random random = new Random();
                int randomIndex = random.Next(advises.Count);
                Advice randomValue = advises[randomIndex];
                var currentDirectory = Directory.GetCurrentDirectory();
                var relativePath = Path.Combine(currentDirectory, randomValue.Photo);

                await client.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать в нашего бота о путешествиях!😊\n\n " +
                    "Я помогу вам узнать интересные факты о разных странах, поделюсь советами по путешествиям и также Вы можете пройти викторину," +
                    "а также интерактивный опрос и мы обработаем ответы пользователей и предоставьте дополнительную информацию 🌍✈️");
                await client.SendTextMessageAsync(message.Chat.Id,randomValue.Title+"\n"+randomValue.Text );
               // using var stream = new FileStream(randomValue.Photo, FileMode.Open);
               // var file = new InputFile(stream);
                //await client.SendPhotoAsync(chatId, file);
                var m = await client.SendPhotoAsync(chatId, relativePath);



                // Отправляем встроенное меню при команде /start
                await client.SendTextMessageAsync(message.Chat.Id, "Выберите действие:", replyMarkup: keyboard);

            }
        }
    }
}
