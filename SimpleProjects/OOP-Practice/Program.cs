using Spectre.Console;




while (true)
{
    var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuChoices>()
                    .Title("What do you want to do next?")
                    .AddChoices(Enum.GetValues<MenuChoices>()));
    switch (choice)
    {
        case MenuChoices.ViewBooks:
            break;
        case MenuChoices.AddBook:
            break;
        case MenuChoices.DeleteBook:
            break;
    }
}



enum MenuChoices
{
    ViewBooks,
    AddBook,
    DeleteBook
}
