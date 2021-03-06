﻿using System;
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
        #region Data members

        private int score;
        private int level = 1;

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private GameManager gameManager;
        private readonly GameViewModel gameViewModel;
        private readonly GameEndDialog gameEndDialog;

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

            this.gameViewModel = new GameViewModel();
            this.gameEndDialog = new GameEndDialog {DataContext = this.gameViewModel};

            this.setupNewGame();
        }

        #endregion

        #region Methods

        private void deathByWallElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.deathByWallElement.Stop();
        }

        private void deathByWaterElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.deathByWaterElement.Stop();
        }

        private void deathByTimeRunoutElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.deathByTimeRunoutElement.Stop();
        }

        private void deathByVehicleElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.deathByVehicleElement.Stop();
        }

        private void GameOverElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            this.gameOverElement.Stop();
        }

        private void PowerUpElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.powerUpActivatedElement.Stop();
        }

        private void MadeItHomeElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            this.madeItHomeElement.Stop();
        }

        private void LevelCompleteElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.levelCompleteElement.Stop();
        }

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

        private void onScoreCountUpdated(object sender, ScoreIncreasedEventArgs scoreIncreased)
        {
            this.madeItHomeElement.IsMuted = false;
            this.madeItHomeElement.Play();
            this.score = scoreIncreased.Score;
            this.scoreTextBlock.Text = "Score: " + this.score;
        }

        private void onLivesCountUpdated(object sender, LivesLostEventArgs lives)
        {
            this.livesTextBlock.Text = "Lives: " + lives.Lives;
        }

        private void onLevelUpdated(object sender, LevelIncreasedEventArgs levelIncreased)
        {
            this.madeItHomeElement.IsMuted = true;
            this.levelCompleteElement.IsMuted = false;
            this.levelCompleteElement.Play();
            this.level = levelIncreased.Level;
            this.levelTextBlock.Text = "Level: " + this.level;
        }

        private async void onGameOver(object sender, EventArgs e)
        {
            this.gameOverTextBlock.Visibility = Visibility.Visible;
            this.backgroundMusicElement.Stop();

            this.gameViewModel.SetScoreLevel(this.gameManager.Score, this.gameManager.Level);

            this.muteDeathSoundEffects();
            this.gameOverElement.Play();

            var result = await this.showGameEndContentDialog();

            if (result == ContentDialogResult.Primary)
            {
                this.restart();
            }
            else if (result == ContentDialogResult.Secondary)
            {
                closeGame();
            }
        }

        private void muteDeathSoundEffects()
        {
            this.deathByWaterElement.IsMuted = true;
            this.deathByTimeRunoutElement.IsMuted = true;
            this.deathByVehicleElement.IsMuted = true;
            this.deathByWallElement.IsMuted = true;
        }

        private void unmuteDeathSoundEffects()
        {
            this.deathByWaterElement.IsMuted = false;
            this.deathByTimeRunoutElement.IsMuted = false;
            this.deathByVehicleElement.IsMuted = false;
            this.deathByWallElement.IsMuted = false;
        }

        private void onTimeRemainingUpdate(object sender, TimeRemainingEventArgs timeRemaining)
        {
            this.timeRemainingTextBlock.Text = "Time: " + timeRemaining.TimeRemaining;
        }

        private void onPowerUpActivated(object sender, EventArgs e)
        {
            this.powerUpActivatedElement.Play();
        }

        private async Task<ContentDialogResult> showGameEndContentDialog()
        {
            var result = await this.gameEndDialog.ShowAsync();

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

        private async void setupNewGame()
        {
            this.gameManager = new GameManager(this.applicationHeight, this.applicationWidth);
            this.gameManager.InitializeGame(this.canvas);

            this.setupEvents();
            this.resetTextBlocks();
            this.gameEndDialog.Reset();

            await this.showStartDialog();

            this.unmuteDeathSoundEffects();
        }

        private async Task showStartDialog()
        {
            var startDialog = new StartScreenDialog();
            var result = await startDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.gameManager.StartGame();
                this.backgroundMusicElement.Play();
            }
        }

        private async Task showNoHighScoresScreen()
        {
            var noHighScoresScreen = new NoHighScoresToShowDialog();
            var result = await noHighScoresScreen.ShowAsync();

            if (result == ContentDialogResult.Secondary)
            {
                await this.chooseFileAndSetHighScores();
            }
        }

        private async Task chooseFileAndSetHighScores()
        {
            var highScores = await HighScoreFileReader.ReadHighScoresFile();
            this.gameViewModel.HighScores = highScores.ToObservableCollection();
        }

        private void setupEvents()
        {
            this.gameManager.ScoreIncreased += this.onScoreCountUpdated;
            this.gameManager.LifeLost += this.onLivesCountUpdated;
            this.gameManager.GameOver += this.onGameOver;
            this.gameManager.TimeRemainingCount += this.onTimeRemainingUpdate;
            this.gameManager.LevelIncreased += this.onLevelUpdated;
            this.gameManager.PowerUpActivated += this.onPowerUpActivated;

            this.gameManager.DiedHitByVehicle += this.onDiedByVehicle;
            this.gameManager.DiedHitWall += this.onDiedHitWall;
            this.gameManager.DiedInWater += this.onDiedInWater;
            this.gameManager.DiedTimeRanOut += this.onDiedTimeRunout;
        }

        private void onDiedTimeRunout(object sender, EventArgs e)
        {
            this.deathByTimeRunoutElement.Play();
        }

        private void onDiedInWater(object sender, EventArgs e)
        {
            this.deathByWaterElement.Play();
        }

        private void onDiedHitWall(object sender, EventArgs e)
        {
            this.deathByWallElement.Play();
        }

        private void onDiedByVehicle(object sender, EventArgs e)
        {
            this.deathByVehicleElement.Play();
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