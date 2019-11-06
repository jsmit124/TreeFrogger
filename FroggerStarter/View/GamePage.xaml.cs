using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Constants;
using FroggerStarter.Controller;
using static FroggerStarter.Controller.GameManager;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = this.applicationWidth, Height = this.applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                           .SetPreferredMinSize(new Size(this.applicationWidth, this.applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            this.setupNewGame();
        }

        #endregion

        #region Methods

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.MovePlayerDown();
                    break;
            }
        }

        private void onScoreCountUpdated(object sender, ScoreIncreasedEventArgs score)
        {
            this.scoreTextBlock.Text = "Score: " + (score.Score + 1);
        }

        private void onLivesCountUpdated(object sender, LivesLostEventArgs lives)
        {
            this.livesTextBlock.Text = "Lives: " + lives.Lives;
        }

        private void onLevelUpdated(object sender, LevelIncreasedEventArgs level)
        {
            this.levelTextBlock.Text = "Level: " + level.Level;
        }

        private async void onGameOver(object sender, EventArgs e)
        {
            this.gameOverTextBlock.Visibility = Visibility.Visible;

            var result = await showGameEndContentDialog();

            if (result == ContentDialogResult.Primary)
            {
                this.restart();
            }
            else
            {
                closeGame();
            }
        }

        private void onTimeRemainingUpdate(object sender, TimeRemainingEventArgs timeRemaining)
        {
            this.timeRemainingTextBlock.Text = "Time: " + timeRemaining.TimeRemaining;
        }

        private static async Task<ContentDialogResult> showGameEndContentDialog()
        {
            var gameEndDialog = new ContentDialog {
                Title = "GAME OVER",
                Content = "Play again?",
                PrimaryButtonText = "Play Again",
                CloseButtonText = "Close"
            };
            var result = await gameEndDialog.ShowAsync();

            return result;
        }

        private void restart()
        {
            this.gameManager.RemoveSprites();
            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
            this.setupNewGame();
        }

        private static void closeGame()
        {
            Application.Current.Exit();
        }

        private void setupNewGame()
        {
            this.gameManager = new GameManager(this.applicationHeight, this.applicationWidth);
            this.gameManager.InitializeGame(this.canvas);

            this.gameManager.ScoreIncreased += this.onScoreCountUpdated;
            this.gameManager.LifeLost += this.onLivesCountUpdated;
            this.gameManager.GameOver += this.onGameOver;
            this.gameManager.TimeRemainingCount += this.onTimeRemainingUpdate;
            this.gameManager.LevelIncreased += this.onLevelUpdated;

            this.resetTextBlocks();
        }

        private void resetTextBlocks()
        {
            this.scoreTextBlock.Text = "Score: 0";
            this.livesTextBlock.Text = "Lives: " + GameSettings.PlayerLives;
            this.levelTextBlock.Text = "Level: 1";
            this.timeRemainingTextBlock.Text = "Time: " + GameSettings.TimeRemainingAtStart;
        }

        #endregion
    }
}