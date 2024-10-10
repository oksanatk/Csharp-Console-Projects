using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Spectre.Console;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

string? speechKey = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Key");
string? speechRegion = Environment.GetEnvironmentVariable("Azure_SpeechSDK_Region");

var products = new List<Product>
{
    new Product(1, "Espresso", 4.5),
    new Product(2, "Cappuccino", 5.5),
    new Product(3, "Latte", 6),
    new Product(4, "Americano", 4.5),
    new Product(5, "Half Caffeinated", 4.5),
    new Product(6, "Mocha", 6),
    new Product(7, "Cold Brew", 5)
};

List<Product> order = new List<Product>();

await GetOrder();
Panel CreateMenuPanel()
{
    var table = new Table().Centered();
    table.AddColumn("[bold yellow]Name[/]");
    table.AddColumn("[bold yellow]Price[/]");

    foreach (Product p in products)
    {
        table.AddRow($"[green]{p.Name}[/]", $"[blue]{p.Price.ToString("C")}[/]");
    }

    return new Panel(table)
    {
        Header = new PanelHeader("Our Menu"),
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.DeepSkyBlue2),
        Padding = new Padding(1)
    };
}

async Task GetOrder()
{
    var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
    speechConfig.SpeechRecognitionLanguage = "en-US";

    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    using SpeechRecognizer recognizer = new SpeechRecognizer(speechConfig, audioConfig);

    bool continueOrdering = true;

    do
    {
        Console.Clear();
        ShowGrid();
        Console.WriteLine("What would you like to order?");
        Console.WriteLine("OR say 'finish' if you'd like to finish your order.");

        bool productRecognized = false;
        while (!productRecognized)
        {
            var result = await recognizer.RecognizeOnceAsync();
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine(result.Text);

                string userChoice = Regex.Replace(result.Text.Trim().ToLower(), @"[^a-z0-9\s]", "");

                if (userChoice == "finish")
                {
                    continueOrdering = false;
                    break;
                }

                var selectedProduct = products.FirstOrDefault(p => p.Name.ToLower() == userChoice);

                if (selectedProduct != null)
                {
                    order.Add(selectedProduct);
                    double totalPrice = order.Sum(p => p.Price);
                    Console.WriteLine($"\nAdded {selectedProduct.Name} to your order. Current order total: {totalPrice:C}");

                } else
                {
                    Console.WriteLine("Sorry, that product we couldn't recognize that product name.");
                }
                productRecognized = true;
            }   
        }

    } while (continueOrdering);

    double finalTotalPrice = order.Sum(p => p.Price);
    Console.WriteLine($"Your total price is: {finalTotalPrice:C}");
    Console.WriteLine("Your order is complete! Your coffee will be out soon.");
}

void ShowGrid()
{
    var menuPanel = CreateMenuPanel();
    var orderPanel = CreateOrderPanel(order);

    Grid grid = new Grid();
    grid.AddColumns(2);
    grid.AddRow(menuPanel, orderPanel);

    AnsiConsole.Console.Write(grid);
}

Panel CreateOrderPanel(List<Product> order)
{
    var table = new Table().Centered();
    table.AddColumn("[bold yellow]Name[/]");
    table.AddColumn("[bold yellow]Price[/]");

    foreach (Product p in order)
    {
        table.AddRow($"[green]{p.Name}[/]", $"[blue]{p.Price.ToString("C")}[/]");
    }
    double totalPrice = order.Sum(p => p.Price);
    table.AddEmptyRow();
    table.AddRow("[bold yellow]Total:[/]", $"[bold red]{totalPrice:C}[/]");

    return new Panel(table)
    {
        Header = new PanelHeader("[underline cyan]Order List[/]"),
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.DeepSkyBlue2),
        Padding = new Padding(1)
    };
}

record Product(int id, string Name, double Price);