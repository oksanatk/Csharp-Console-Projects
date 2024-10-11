using Microsoft.CognitiveServices.Speech;
using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;

string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");
Console.OutputEncoding = System.Text.Encoding.Unicode;

List<BakedGood> menuChoices = new List<BakedGood>()
{
    new BakedGood(1, "Пирожок с Капустой", 70),
    new BakedGood(2, "Пирожок с Яйцом", 70),
    new BakedGood(3, "Пирожок с Яблоком", 70),
    new BakedGood(4, "Ежик с Ягодами", 75),
    new BakedGood(5, "Булочка с Маком", 50),
    new BakedGood(6, "Булочка со Смородиной", 90),
    new BakedGood(7, "Песочный Лимонник", 105),
    new BakedGood(8, "Хлеб по Деревенский", 100)
};

List<BakedGood> order = new List<BakedGood>();

await GetOrder();

async Task GetOrder()
{
    SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
    speechConfig.SpeechRecognitionLanguage = "ru-RU";
    using var recognizer = new SpeechRecognizer(speechConfig);

    bool continueOrdering = true;

    while (continueOrdering)
    {
        Console.Clear();
        ShowGrid();
        Console.WriteLine("Что вы хотите заказать?");
        Console.WriteLine("Или, зкажите 'Закончил(а)' чтобы законьчить Ваш заказ.");

        bool productRecognized = false;
        while (!productRecognized)
        {
            var result = await recognizer.RecognizeOnceAsync();
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine(result.Text);

                string userChoice = result.Text.Trim().ToLower();
                userChoice = Regex.Replace(userChoice, @"[^a-z0-9\u0400-\u04FF\s]", "");

                if (userChoice.StartsWith("закон")) 
                {
                    continueOrdering = false;
                    break;
                }

                var selectedGood = menuChoices.FirstOrDefault(b => b.Name.ToLower() == userChoice);

                if (selectedGood != null)
                {
                    order.Add(selectedGood);
                    double totalPrice = order.Sum(good => good.Price);
                    Console.WriteLine($"\nМы добавили {selectedGood.Name} к Вашему заказу. Ваша сумма: {selectedGood.Price.ToString("C",CultureInfo.CreateSpecificCulture("ru"))}");

                }else
                {
                    Console.WriteLine("Извините, но мы не узнали названье такого товара.");
                }
                productRecognized = true;
            }
        }
    }

    CreateOrderPanel(order);

    double finalTotalPrice = order.Sum(good => good.Price);
    Console.WriteLine($"Ваша оканьчательная сумма: {finalTotalPrice.ToString("C", CultureInfo.CreateSpecificCulture("ru"))}");
    Console.WriteLine("Ваши товары скора будут готовы!");
}
void ShowGrid()
{
    var menuPanel = CreateMenuPanel();
    var orderPanel = CreateOrderPanel(order);

    var grid = new Grid();
    grid.AddColumns(2);
    grid.AddRow(menuPanel, orderPanel);

    AnsiConsole.Console.Write(grid);
}

Panel CreateMenuPanel()
{
    Table table = new Table().Centered();
    table.AddColumn("[bold yellow]Товар[/]");
    table.AddColumn("[bold yellow]Цена[/]");

    foreach (BakedGood treat in menuChoices)
    {
        table.AddRow($"[green]{treat.Name}[/]", $"[blue]{treat.Price.ToString("C", CultureInfo.CreateSpecificCulture("ru"))}[/]");
    }

    return new Panel(table)
    {
        Header = new PanelHeader("Наше Меню:"),
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.DeepSkyBlue2),
        Padding = new Padding(1)
    };
}

Panel CreateOrderPanel(List<BakedGood> order)
{
    Table table = new Table().Centered();
    table.AddColumn("[bold yellow]Товар[/]");
    table.AddColumn("[bold yellow]Цена[/]");

    foreach(BakedGood treat in order)
    {
        table.AddRow($"[green]{treat.Name}[/]", $"[blue]{treat.Price.ToString("C", CultureInfo.CreateSpecificCulture("ru"))}[/]");
    };
    table.AddEmptyRow();
    double totalPrice = order.Sum(good  => good.Price);
    table.AddRow($"[bold red]Ваша сумма: [/]", $"[bold red]{totalPrice.ToString("C", CultureInfo.CreateSpecificCulture("ru"))}[/]");

    return new Panel(table)
    {
        Header = new PanelHeader("Ваш Заказ"),
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.DeepSkyBlue2),
        Padding = new Padding(1)
    };
}

record BakedGood (int id, string Name, double Price);