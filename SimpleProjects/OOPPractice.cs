using System;
using Microsoft.CognitiveServices.Speech;
using Spectre.Console;

internal class OOPPractice
{
	internal OOPPractice()
	{
		while (true)
		{
			var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuOption>()
									.Title("What do you want to do next?")
									.AddChoices(Enum.GetValues<MenuOption>()));
		}
	}
}

enum MenuOption
{
	ViewBooks,
	AddBook,
	DeleteBook
}
	

