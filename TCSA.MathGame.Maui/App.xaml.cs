using TCSA.MathGame.Maui.Data;

namespace TCSA.MathGame.Maui
{
    public partial class App : Application
    {
        public static GameRepository GameRepository { get; set; }
        public App(GameRepository gameRepository)
        {
            InitializeComponent();

            GameRepository = gameRepository;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

    }
}