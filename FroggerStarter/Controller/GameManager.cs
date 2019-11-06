﻿using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Constants;
using FroggerStarter.Model;
using FroggerStarter.View;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the playerSprite,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private bool roundOneOver;
        private bool roundTwoOver;
        private bool roundThreeOver;
        private bool gameIsOver;
        private readonly double topLaneYLocation = (double) Application.Current.Resources["HighRoadYLocation"];

        private Canvas gameCanvas;

        private readonly LaneManager laneManager;
        private readonly FrogHomeManager homeManager;
        private readonly DeathAnimationManager deathAnimationManager;
        private readonly TimerPowerUpManager powerUpManager;
        private readonly PlayerManager playerManager;

        private DispatcherTimer gameTimer;
        private DispatcherTimer timeRemainingTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="backgroundHeight">Height of the background.</param>
        /// <param name="backgroundWidth">Width of the background.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     backgroundHeight &lt;= 0
        ///     or
        ///     backgroundWidth &lt;= 0
        /// </exception>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            this.laneManager = new LaneManager(LaneSettings.TopLaneYLocation);
            this.homeManager = new FrogHomeManager(LaneSettings.TopLaneYLocation);
            this.deathAnimationManager = new DeathAnimationManager();
            this.powerUpManager = new TimerPowerUpManager();
            this.playerManager = new PlayerManager();

            this.deathAnimationManager.AnimationOver += this.handleDeathAnimationHasEnded;
            this.handleStartup();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Initializes the game working with appropriate classes to play frog
        ///     and vehicle on game screen.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createAndPlacePlayer();
            this.addPowerUpsToView();
            this.addVehiclesToView();
            this.addDeathAnimationsToView();
            this.placeHomeFrogs();
            this.laneManager.HideVehicles();
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MovePlayerLeft(0);
        }

        /// <summary>
        ///     Moves the playerSprite to the right.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev + playerSprite.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MovePlayerRight(this.backgroundWidth);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.playerManager.MovePlayerUp(this.topLaneYLocation);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.playerManager.MovePlayerDown((int) Math.Floor(this.backgroundHeight));
        }

        /// <summary>
        ///     Removes the sprites.
        ///     Precondition: None
        ///     Postcondition: Sprites removed from canvas children
        /// </summary>
        public void RemoveSprites()
        {
            this.gameCanvas.Children.Remove(this.playerManager.Sprite);
            this.removePowerUpSprites();
            this.removeVehicleSprites();
            this.removeHomeFrogSprites();
        }

        #endregion

        #region TimerOnTick Methods

        private void gameTimerOnTick(object sender, object e)
        {
            this.laneManager.MoveVehicles();
            this.checkForPlayerCollisionWithObjects();
            this.checkForPlayerHitTopWall();
        }

        private void timeRemainingTimerOnTick(object sender, object e)
        {
            this.playerManager.DecrementTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerManager.TimeRemaining};
            this.TimeRemainingCount?.Invoke(this, timeRemaining);

            if (this.playerManager.TimeRemaining == 0)
            {
                this.handleTimeRemainingIsZero();
            }
        }

        #endregion

        #region Checker Methods

        private void checkForPlayerCollisionWithObjects()
        {
            this.checkForPlayerCollisionWithVehicles();
            this.checkForPlayerCollisionWithHome();
            this.checkForPlayerCollisionWithTimerPowerUp();
        }

        private void checkForPlayerCollisionWithVehicles()
        {
            foreach (var vehicle in this.laneManager)
            {
                if (this.playerManager.CollisionDetected(vehicle) && vehicle.Sprite.Visibility != Visibility.Collapsed)
                {
                    this.handleLifeLost();
                }
            }
        }

        private void checkForPlayerCollisionWithHome()
        {
            foreach (var frogHome in this.homeManager)
            {
                if (this.playerManager.CollisionDetected(frogHome))
                {
                    this.handleFrogMadeItHome(frogHome);
                }
            }
        }

        private void checkForPlayerCollisionWithTimerPowerUp()
        {
            foreach (var powerUp in this.powerUpManager)
            {
                if (this.playerManager.CollisionDetected(powerUp) && powerUp.Sprite.Visibility != Visibility.Collapsed)
                {
                    this.handleFrogTimerPowerUp(powerUp);
                }
            }
        }

        private void checkForLevelOver()
        {
            if (this.playerManager.AmountOfFrogsInHome != GameSettings.FrogHomeCount)
            {
                return;
            }

            this.handleLevelOver();
        }

        private bool checkForGameOver()
        {
            if (this.playerManager.Lives != 0 && !this.roundThreeOver)
            {
                return false;
            }

            this.handleGameOver();
            return true;
        }

        private void checkForPlayerHitTopWall()
        {
            if (this.playerManager.Y <= this.topLaneYLocation)
            {
                this.handleLifeLost();
            }
        }

        #endregion

        #region Handler Methods

        private async void handleStartup()
        {
            var startDialog = new StartScreenDialog();
            var result = await startDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.setupGameTimer();
                this.setupTimeRemainingTimer();
                this.powerUpManager.startPowerUpTimer();
                this.playerManager.EnableMovement();
            }
        }

        private void handleLevelOver()
        {
            if (!this.roundOneOver)
            {
                this.roundOneOver = true;
                this.enableNextLevel();
            }
            else if (!this.roundTwoOver)
            {
                this.roundTwoOver = true;
                this.enableNextLevel();
            }
            else if (!this.roundThreeOver)
            {
                this.roundThreeOver = true;
                this.handleGameOver();
            }
            this.playerManager.IncrementLevel();
            var level = new LevelIncreasedEventArgs { Level = this.playerManager.Level };
            this.LevelIncreased?.Invoke(this, level);
        }

        private void handleLifeLost()
        {
            this.playerManager.Sprite.Visibility = Visibility.Collapsed;
            this.playerManager.DecrementLives();

            var lives = new LivesLostEventArgs {Lives = this.playerManager.Lives};
            this.LifeLost?.Invoke(this, lives);

            this.timeRemainingTimer.Stop();
            this.handleStartDeathAnimation();

            if (!this.checkForGameOver())
            {
                this.setPlayerToCenterOfBottomLane();
            }
        }

        private void handleStartDeathAnimation()
        {
            this.playerManager.DisableMovement();
            this.playerManager.Sprite.Visibility = Visibility.Collapsed;

            this.setDeathAnimationToPlayerLocation();
            this.deathAnimationManager.PlayDeathAnimation();
        }

        private void handlePlayerScored()
        {
            this.playerManager.IncrementScore(this.playerManager.TimeRemaining);

            var score = new ScoreIncreasedEventArgs {Score = this.playerManager.Score};
            this.ScoreIncreased?.Invoke(this, score);

            this.resetTimeRemainingTimer();
            this.setPlayerToCenterOfBottomLane();
        }

        private void handleGameOver()
        {
            this.gameIsOver = true;
            this.gameTimer.Stop();
            this.playerManager.Sprite.Visibility = Visibility.Collapsed;
            this.removePowerUpSprites();
            this.playerManager.DisableMovement();
            this.timeRemainingTimer.Stop();
            this.powerUpManager.stopPowerUpTimer();
            this.GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void handleTimeRemainingIsZero()
        {
            this.handleLifeLost();

            if (!this.checkForGameOver())
            {
                this.resetTimeRemainingTimer();
            }
        }

        private void handleFrogMadeItHome(FrogHome frogHome)
        {
            if (frogHome.Sprite.Visibility == Visibility.Visible)
            {
                this.handleLifeLost();
            }
            else
            {
                this.handlePlayerScored();
                frogHome.Sprite.Visibility = Visibility.Visible;
                this.playerManager.IncrementFrogsInHomes();
                this.checkForGameOver();
                this.checkForLevelOver();
            }
        }

        private void handleDeathAnimationHasEnded(object sender, EventArgs e)
        {
            this.deathAnimationManager.CollapseAllAnimationFrames();
            this.deathAnimationManager.StopAnimationTimer();
            this.deathAnimationManager.ResetFrameCount();

            if (this.gameIsOver)
            {
                return;
            }

            this.playerManager.Sprite.Visibility = Visibility.Visible;
            this.playerManager.EnableMovement();
            this.resetTimeRemainingTimer();
            this.laneManager.HideVehicles();
        }

        private void handleFrogTimerPowerUp(TimerPowerUp powerUp)
        {
            this.playerManager.TimerPowerUp();
            powerUp.Sprite.Visibility = Visibility.Collapsed;
            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerManager.TimeRemaining};
            this.TimeRemainingCount?.Invoke(this, timeRemaining);
        }

        #endregion

        #region Add To View Methods

        private void createAndPlacePlayer()
        {
            this.gameCanvas.Children.Add(this.playerManager.Sprite);
            this.setPlayerToCenterOfBottomLane();
            this.playerManager.DisableMovement();
        }

        private void placeHomeFrogs()
        {
            this.homeManager.ToList().ForEach(homeFrog => this.gameCanvas.Children.Add(homeFrog.Sprite));
        }

        private void addVehiclesToView()
        {
            this.laneManager.ToList().ForEach(vehicle => this.gameCanvas.Children.Add(vehicle.Sprite));
        }

        private void addDeathAnimationsToView()
        {
            this.deathAnimationManager.ToList().ForEach(animation => this.gameCanvas.Children.Add(animation.Sprite));
        }

        private void addPowerUpsToView()
        {
            this.powerUpManager.ToList().ForEach(powerUp => this.gameCanvas.Children.Add(powerUp.Sprite));
        }

        #endregion

        #region Remove From View Methods

        private void removeHomeFrogSprites()
        {
            this.homeManager.ToList().ForEach(homeFrog => this.gameCanvas.Children.Remove(homeFrog.Sprite));
        }

        private void removeVehicleSprites()
        {
            this.laneManager.ToList().ForEach(vehicle => this.gameCanvas.Children.Remove(vehicle.Sprite));
        }

        private void removePowerUpSprites()
        {
            this.powerUpManager.ToList().ForEach(powerUp => this.gameCanvas.Children.Remove(powerUp.Sprite));
        }

        #endregion

        #region Private Helper Methods

        private void setDeathAnimationToPlayerLocation()
        {
            this.deathAnimationManager.SetAnimationLocation(this.playerManager.X, this.playerManager.Y);
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.playerManager.SetPlayerToCenterOfBottomLane();
        }

        private void resetTimeRemainingTimer()
        {
            this.timeRemainingTimer.Stop();
            this.playerManager.ResetTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerManager.TimeRemaining};
            this.TimeRemainingCount?.Invoke(this, timeRemaining);

            this.timeRemainingTimer.Start();
        }

        private void enableNextLevel()
        {
            this.homeManager.makeHomeFrogsCollapsed();
            this.playerManager.ResetFrogsInHome();
            this.laneManager.IncreaseAllVehicleSpeed(.4);
            this.laneManager.IncrementMaxAmountOfVehiclesPerLane();
        }

        #endregion

        #region Timer Setups

        private void setupGameTimer()
        {
            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Tick += this.gameTimerOnTick;
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.gameTimer.Start();
        }

        private void setupTimeRemainingTimer()
        {
            this.timeRemainingTimer = new DispatcherTimer();
            this.timeRemainingTimer.Tick += this.timeRemainingTimerOnTick;
            this.timeRemainingTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.timeRemainingTimer.Start();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        ///     The game over event handler
        /// </summary>
        public EventHandler<EventArgs> GameOver;

        /// <summary>
        ///     The life lost event handler
        /// </summary>
        public EventHandler<LivesLostEventArgs> LifeLost;

        /// <summary>
        ///     The time remaining count event handler
        /// </summary>
        public EventHandler<TimeRemainingEventArgs> TimeRemainingCount;

        /// <summary>
        ///     The score increased event handler
        /// </summary>
        public EventHandler<ScoreIncreasedEventArgs> ScoreIncreased;

        /// <summary>
        ///     The level increased event handler
        /// </summary>
        public EventHandler<LevelIncreasedEventArgs> LevelIncreased;

        #endregion

        #region EventArgs Classes

        /// <summary>
        ///     Handles the event arguments for the Lives Lost event
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class LivesLostEventArgs : EventArgs
        {
            #region Properties

            /// <summary>
            ///     Gets or sets the lives.
            /// </summary>
            /// <value>
            ///     The lives.
            /// </value>
            public int Lives { get; set; }

            #endregion
        }

        /// <summary>
        ///     Handles the event arguments for the level increased event
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class LevelIncreasedEventArgs : EventArgs
        {
            #region Properties

            /// <summary>
            ///     Gets or sets the level.
            /// </summary>
            /// <value>
            ///     The level.
            /// </value>
            public int Level { get; set; }

            #endregion
        }

        /// <summary>
        ///     Handles the event arguments for the Time Remaining event
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class TimeRemainingEventArgs : EventArgs
        {
            #region Properties

            /// <summary>
            ///     Gets or sets the time remaining.
            /// </summary>
            /// <value>
            ///     The time remaining.
            /// </value>
            public int TimeRemaining { get; set; }

            #endregion
        }

        /// <summary>
        ///     Handles the event arguments for the Score Increased event
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class ScoreIncreasedEventArgs : EventArgs
        {
            #region Properties

            /// <summary>
            ///     Gets or sets the score.
            /// </summary>
            /// <value>
            ///     The score.
            /// </value>
            public int Score { get; set; }

            #endregion
        }

        #endregion
    }
}