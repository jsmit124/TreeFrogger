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
using FroggerStarter.Extensions;
using FroggerStarter.IO;
using FroggerStarter.View.Dialogs;
using FroggerStarter.ViewModel;
using static FroggerStarter.Controller.GameManager;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = applicationWidth, Height = applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                .SetPreferredMinSize(new Size(applicationWidth, applicationHeight));

            Window.Current.CoreWindow.KeyDown += coreWindowOnKeyDown;

            gameViewModel = new GameViewModel();
            gameEndDialog = new GameEndDialog {DataContext = gameViewModel};

            setupNewGame();
        }

        #endregion

        #region Data members

        private int score;
        private int level = 1;

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private GameManager gameManager;
        private readonly GameViewModel gameViewModel;
        private readonly GameEndDialog gameEndDialog;

        #endregion

        #region Methods

        private void deathByWallElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            deathByWallElement.Stop();
        }

        private void deathByWaterElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            deathByWaterElement.Stop();
        }

        private void deathByTimeRunoutElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            deathByTimeRunoutElement.Stop();
        }

        private void deathByVehicleElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            deathByVehicleElement.Stop();
        }

        private void GameOverElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            gameOverElement.Stop();
        }

        private void PowerUpElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            powerUpActivatedElement.Stop();
        }

        private void MadeItHomeElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            madeItHomeElement.Stop();
        }

        private void LevelCompleteElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            levelCompleteElement.Stop();
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    gameManager.MovePlayerDown();
                    break;
            }
        }

        private void onScoreCountUpdated(object sender, ScoreIncreasedEventArgs scoreIncreased)
        {
            madeItHomeElement.IsMuted = false;
            madeItHomeElement.Play();
            score = scoreIncreased.Score;
            scoreTextBlock.Text = "Score: " + score;
        }

        private void onLivesCountUpdated(object sender, LivesLostEventArgs lives)
        {
            livesTextBlock.Text = "Lives: " + lives.Lives;
        }

        private void onLevelUpdated(object sender, LevelIncreasedEventArgs levelIncreased)
        {
            madeItHomeElement.IsMuted = true;
            levelCompleteElement.IsMuted = false;
            levelCompleteElement.Play();
            level = levelIncreased.Level;
            levelTextBlock.Text = "Level: " + level;
        }

        private async void onGameOver(object sender, EventArgs e)
        {
            gameOverTextBlock.Visibility = Visibility.Visible;
            backgroundMusicElement.Stop();

            muteDeathSoundEffects();
            gameOverElement.Play();

            var result = await showGameEndContentDialog();

            if (result == ContentDialogResult.Primary)
                restart();
            else if (result == ContentDialogResult.Secondary) closeGame();
        }

        private void muteDeathSoundEffects()
        {
            deathByWaterElement.IsMuted = true;
            deathByTimeRunoutElement.IsMuted = true;
            deathByVehicleElement.IsMuted = true;
            deathByWallElement.IsMuted = true;
        }

        private void unmuteDeathSoundEffects()
        {
            deathByWaterElement.IsMuted = false;
            deathByTimeRunoutElement.IsMuted = false;
            deathByVehicleElement.IsMuted = false;
            deathByWallElement.IsMuted = false;
        }

        private void onTimeRemainingUpdate(object sender, TimeRemainingEventArgs timeRemaining)
        {
            timeRemainingTextBlock.Text = "Time: " + timeRemaining.TimeRemaining;
        }

        private void onPowerUpActivated(object sender, EventArgs e)
        {
            powerUpActivatedElement.Play();
        }

        private async Task<ContentDialogResult> showGameEndContentDialog()
        {
            var result = await gameEndDialog.ShowAsync();

            return result;
        }

        private void restart()
        {
            gameManager.RemoveSprites();
            gameOverTextBlock.Visibility = Visibility.Collapsed;
            setupNewGame();
        }

        private static void closeGame()
        {
            Application.Current.Exit();
        }

        private async void setupNewGame()
        {
            gameManager = new GameManager(applicationHeight, applicationWidth);
            gameManager.InitializeGame(canvas);

            setupEvents();
            resetTextBlocks();
            gameEndDialog.Reset();

            await showStartDialog();

            unmuteDeathSoundEffects();
        }

        private async Task showStartDialog()
        {
            var startDialog = new StartScreenDialog();
            var result = await startDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                gameManager.StartGame();
                backgroundMusicElement.Play();
            }
        }

        private async Task showNoHighScoresScreen()
        {
            var noHighScoresScreen = new NoHighScoresToShowDialog();
            var result = await noHighScoresScreen.ShowAsync();

            if (result == ContentDialogResult.Secondary) await chooseFileAndSetHighScores();
        }

        private async Task chooseFileAndSetHighScores()
        {
            var highScores = await HighScoreFileReader.ReadHighScoresFile();
            gameViewModel.HighScores = highScores.ToObservableCollection();
        }

        private void setupEvents()
        {
            gameManager.ScoreIncreased += onScoreCountUpdated;
            gameManager.LifeLost += onLivesCountUpdated;
            gameManager.GameOver += onGameOver;
            gameManager.TimeRemainingCount += onTimeRemainingUpdate;
            gameManager.LevelIncreased += onLevelUpdated;
            gameManager.PowerUpActivated += onPowerUpActivated;

            gameManager.DiedHitByVehicle += onDiedByVehicle;
            gameManager.DiedHitWall += onDiedHitWall;
            gameManager.DiedInWater += onDiedInWater;
            gameManager.DiedTimeRanOut += onDiedTimeRunout;
        }

        private void onDiedTimeRunout(object sender, EventArgs e)
        {
            deathByTimeRunoutElement.Play();
        }

        private void onDiedInWater(object sender, EventArgs e)
        {
            deathByWaterElement.Play();
        }

        private void onDiedHitWall(object sender, EventArgs e)
        {
            deathByWallElement.Play();
        }

        private void onDiedByVehicle(object sender, EventArgs e)
        {
            deathByVehicleElement.Play();
        }

        private void resetTextBlocks()
        {
            scoreTextBlock.Text = "Score: 0";
            livesTextBlock.Text = "Lives: " + GameSettings.PlayerLives;
            levelTextBlock.Text = "Level: 1";
            timeRemainingTextBlock.Text = "Time: " + GameSettings.TimeRemainingAtStart;
        }

        #endregion
    }
}