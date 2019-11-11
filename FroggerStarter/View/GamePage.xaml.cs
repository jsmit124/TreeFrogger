using System;
using System.Collections.ObjectModel;
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
using FroggerStarter.Model;
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
        private int score;
        private int level = 1;
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

            this.gameEndDialog = new GameEndDialog();

            setupNewGame();
            gameViewModel = new GameViewModel();
        }

        private void onAddToHighScoresButtonClicked(object sender, GameEndDialog.AddToHighScoresButtonClickedEventArgs initials)
        {
            this.handleAddToHighScores(initials.Initials);
        }

        private async void onHighScoreButtonClicked(object sender, EventArgs e)
        {
            
            this.gameEndDialog.Hide();
            await Task.Delay(5000);
            await this.handleHighScoresDisplay();
        }

        #endregion

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

        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private GameManager gameManager;
        private readonly GameViewModel gameViewModel;

        private readonly GameEndDialog gameEndDialog;

        #endregion

        #region Methods

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

        private void onScoreCountUpdated(object sender, ScoreIncreasedEventArgs score)
        {
            madeItHomeElement.IsMuted = false;
            madeItHomeElement.Play();
            this.score = score.Score;
            scoreTextBlock.Text = "Score: " + this.score;
        }

        private void onLivesCountUpdated(object sender, LivesLostEventArgs lives)
        {
            livesTextBlock.Text = "Lives: " + lives.Lives;
        }

        private void onLevelUpdated(object sender, LevelIncreasedEventArgs level)
        {
            madeItHomeElement.IsMuted = true;
            levelCompleteElement.IsMuted = false;
            levelCompleteElement.Play();
            this.level = level.Level;
            levelTextBlock.Text = "Level: " + this.level;
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

            //TODO handler add to high scores file and collection within view model
        }

        private void handleAddToHighScores(string initials)
        {
            if (this.gameViewModel.HighScores == null)
            {
                this.gameViewModel.HighScores = new ObservableCollection<HighScorePlayerInfo>();
            }
            this.gameViewModel.HighScores.Add(new HighScorePlayerInfo(initials, this.score, this.level));
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
            var result = await this.gameEndDialog.ShowAsync();

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
            this.gameEndDialog.Reset();

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
            else if (result == ContentDialogResult.Secondary)
            {
                await handleHighScoresDisplay();
            }
        }

        private async Task handleHighScoresDisplay()
        {
            if (gameViewModel.HighScores == null)
                await chooseFileAndSetHighScores();
            else if (gameViewModel.HighScores.Count == 0) await showNoHighScoresScreen();

            if (this.gameViewModel.HighScores != null && gameViewModel.HighScores.Count > 0)
            {
                var highScoresDisplay = new HighScoresDialog();
                await highScoresDisplay.ShowAsync();
            }
            this.restart();
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
            this.gameManager.ScoreIncreased += onScoreCountUpdated;
            this.gameManager.LifeLost += onLivesCountUpdated;
            this.gameManager.GameOver += onGameOver;
            this.gameManager.TimeRemainingCount += onTimeRemainingUpdate;
            this.gameManager.LevelIncreased += onLevelUpdated;
            this.gameManager.PowerUpActivated += onPowerUpActivated;

            this.gameManager.DiedHitByVehicle += onDiedByVehicle;
            this.gameManager.DiedHitWall += onDiedHitWall;
            this.gameManager.DiedInWater += onDiedInWater;
            this.gameManager.DiedTimeRanOut += onDiedTimeRunout;

            this.gameEndDialog.HighScoresButtonClicked += this.onHighScoreButtonClicked;
            this.gameEndDialog.AddToHighScoresButtonClicked += this.onAddToHighScoresButtonClicked;
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