using TCSA.MathGame.Maui.Models;

namespace TCSA.MathGame.Maui;

public partial class GamePage : ContentPage
{
    public string GameType { get; set; }
    int firstNumber = 0;
    int secondNumber = 0;
    int score = 0;
    const int totalQuestions = 5;
    int questionsLeft = totalQuestions;

    public GamePage(string gameType)
    {
        InitializeComponent();
        GameType = gameType;
        BindingContext = this;

        CreateNewQuestion();
    }

    private void CreateNewQuestion()
    {
        Random random = new Random();

        firstNumber = GameType != "÷" ? random.Next(1, 9) : random.Next(1, 99);
        secondNumber = GameType != "÷" ? random.Next(1, 9) : random.Next(1, 99);

        if (GameType == "÷")
        {
            while (firstNumber < secondNumber || firstNumber % secondNumber != 0)
            {
                firstNumber = random.Next(1, 99);
                secondNumber = random.Next(1, 99);
            }
        }

        QuestionLabel.Text = $"{firstNumber} {GameType} {secondNumber}";
    }

    internal void OnAnswerSubmitted(object sender, EventArgs e)
    {
        var answer = Int32.Parse(AnswerEntry.Text);
        bool isCorrect = false;

        switch (GameType)
        {
            case "+":
                isCorrect = answer == firstNumber + secondNumber;
                break;
            case "-":
                isCorrect = answer == firstNumber - secondNumber;
                break;
            case "x":
                isCorrect = answer == firstNumber * secondNumber;
                break;
            case "÷":
                isCorrect = answer == firstNumber / secondNumber;
                break;
        }
        ProcessAnswer(isCorrect);
        questionsLeft--;
        AnswerEntry.Text = "";

        if (questionsLeft > 0)
        {
            CreateNewQuestion();
        } else
        {
            GameOver();
        }
    }

    private void ProcessAnswer(bool isCorrect)
    {
        score = isCorrect ? score+=1 : score;
        AnswerLabel.Text = isCorrect ? "Correct!" : "Incorrect.";
    }

    private void GameOver()
    {
        GameOperation gameOperation = GameType switch
        {
            "+" => GameOperation.Addition,
            "-" => GameOperation.Subtraction,
            "x" => GameOperation.Multiplication,
            "÷" => GameOperation.Division
        };

        QuestionArea.IsVisible = false;
        BackToMenuButton.IsVisible = true;
        GameOverLabel.Text = $"Game Over! You got {score} out of {totalQuestions} correct.";

        App.GameRepository.Add(new Game
        {
            DatePlayed = DateTime.Now,
            Type = gameOperation,
            Score = score
        });
    }

    private void OnBackToMenu(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}