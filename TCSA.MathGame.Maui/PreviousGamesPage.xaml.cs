namespace TCSA.MathGame.Maui;

public partial class PreviousGamesPage : ContentPage
{
    public PreviousGamesPage()
    {
        InitializeComponent();
        gamesList.ItemsSource = App.GameRepository.GetAllGames();
    }

    private void OnDelete(object sender, EventArgs e)
    {
        ImageButton button = (ImageButton)sender;
        App.GameRepository.Delete((int)button.BindingContext);

        gamesList.ItemsSource = App.GameRepository.GetAllGames();
    }
}