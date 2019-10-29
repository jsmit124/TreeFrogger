using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Constants;
using FroggerStarter.Model;

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
        private bool gameIsOver;
        private readonly double topLaneYLocation = (double) Application.Current.Resources["HighRoadYLocation"];

        private Canvas gameCanvas;
        private Frog player;
        private readonly PlayerStatistics playerStats;

        private readonly LaneManager laneManager;
        private readonly HomeFrogManager homeManager;
        private readonly DeathAnimationManager deathAnimationManager;

        private DispatcherTimer gameTimer;
        private DispatcherTimer timeRemainingTimer;
        private DispatcherTimer deathAnimationTimer;

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
            this.laneManager = new LaneManager(this.topLaneYLocation);
            this.homeManager = new HomeFrogManager(this.topLaneYLocation);
            this.deathAnimationManager = new DeathAnimationManager();
            this.playerStats = new PlayerStatistics();

            this.setupGameTimer();
            this.setupTimeRemainingTimer();
            this.setupDeathAnimationTimer();
        }

        #endregion

        #region Methods

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

        private void setupDeathAnimationTimer()
        {
            this.deathAnimationTimer = new DispatcherTimer();
            this.deathAnimationTimer.Tick += this.deathAnimationTimerOnTick;
            this.deathAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void gameTimerOnTick(object sender, object e)
        {
            this.laneManager.MoveVehicles();
            this.checkForPlayerCollisionWithObjects();
            this.checkForPlayerHitTopWall();
        }

        private void timeRemainingTimerOnTick(object sender, object e)
        {
            this.playerStats.DecrementTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerStats.TimeRemaining};
            this.TimeRemainingCount?.Invoke(this, timeRemaining);

            if (this.playerStats.TimeRemaining == 0)
            {
                this.handleTimeRemainingIsZero();
            }
        }

        private void deathAnimationTimerOnTick(object sender, object e)
        {
            if (this.deathAnimationManager.CurrentAnimationFrameIndex > GameSettings.DeathAnimationCount - 1)
            {
                this.handleDeathAnimationHasEnded();
            }
            else
            {
                this.deathAnimationManager.ShowNextFrame();
            }
        }

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
            this.addVehiclesToView();
            this.addDeathAnimationsToView();
            this.placeHomeFrogs();
        }

        private void createAndPlacePlayer()
        {
            this.player = new Frog();
            this.gameCanvas.Children.Add(this.player.Sprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void placeHomeFrogs()
        {
            foreach (var homeFrog in this.homeManager)
            {
                this.gameCanvas.Children.Add(homeFrog.Sprite);
            }
        }

        private void setDeathAnimationToPlayerLocation()
        {
            this.deathAnimationManager.SetAnimationLocation(this.player.X, this.player.Y);
        }

        private void addVehiclesToView()
        {
            foreach (var vehicle in this.laneManager)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
        }

        private void addDeathAnimationsToView()
        {
            foreach (var deathAnimation in this.deathAnimationManager)
            {
                this.gameCanvas.Children.Add(deathAnimation.Sprite);
            }
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.player.X = this.backgroundWidth / 2 - this.player.Width / 2;
            this.player.Y = this.backgroundHeight - this.player.Height - LaneSettings.BottomLaneOffset;
        }

        private void resetTimeRemainingTimer()
        {
            this.timeRemainingTimer.Stop();
            this.playerStats.ResetTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerStats.TimeRemaining};
            this.TimeRemainingCount?.Invoke(this, timeRemaining);

            this.timeRemainingTimer.Start();
        }

        private void playDeathAnimation()
        {
            this.deathAnimationTimer.Start();
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.player.MoveLeftWithBoundaryCheck(0);
        }

        /// <summary>
        ///     Moves the playerSprite to the right.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev + playerSprite.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.player.MoveRightWithBoundaryCheck(this.backgroundWidth);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.player.MoveUpWithBoundaryCheck(this.topLaneYLocation);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.player.MoveDownWithBoundaryCheck((int) Math.Floor(this.backgroundHeight));
        }

        private void checkForPlayerCollisionWithObjects()
        {
            this.checkForPlayerCollisionWithVehicles();
            this.checkForPlayerCollisionWithHome();
        }

        private void checkForPlayerCollisionWithVehicles()
        {
            foreach (var vehicle in this.laneManager)
            {
                if (this.player.CollisionDetected(vehicle))
                {
                    this.handleLifeLost();
                }
            }
        }

        private void checkForPlayerCollisionWithHome()
        {
            foreach (var frogHome in this.homeManager)
            {
                if (this.player.CollisionDetected(frogHome))
                {
                    this.handleFrogMadeItHome(frogHome);
                }
            }
        }

        private bool checkForGameOver()
        {
            if (this.playerStats.Lives != 0 && this.playerStats.AmountOfFrogsInHome != GameSettings.FrogHomeCount)
            {
                return false;
            }

            this.handleGameOver();
            return true;
        }

        private void checkForPlayerHitTopWall()
        {
            if (this.player.Y <= this.topLaneYLocation)
            {
                this.handleLifeLost();
            }
        }

        private void handleLifeLost()
        {
            this.player.Sprite.Visibility = Visibility.Collapsed;
            this.playerStats.DecrementLives();

            var lives = new LivesLostEventArgs {Lives = this.playerStats.Lives};
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
            this.player.StopMovement();
            this.player.Sprite.Visibility = Visibility.Collapsed;

            this.setDeathAnimationToPlayerLocation();
            this.playDeathAnimation();
        }

        private void handlePlayerScored()
        {
            this.playerStats.IncrementScore(this.playerStats.TimeRemaining);

            var score = new ScoreIncreasedEventArgs {Score = this.playerStats.Score};
            this.ScoreIncreased?.Invoke(this, score);

            this.checkForGameOver();
            this.resetTimeRemainingTimer();
            this.setPlayerToCenterOfBottomLane();
        }

        private void handleGameOver()
        {
            this.gameIsOver = true;
            this.gameTimer.Stop();
            this.player.Sprite.Visibility = Visibility.Collapsed;
            this.player.StopMovement();
            this.timeRemainingTimer.Stop();
            this.laneManager.StopAllVehicleMovement();
            this.GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void handleTimeRemainingIsZero()
        {
            this.playerStats.DecrementLives();

            var lives = new LivesLostEventArgs {Lives = this.playerStats.Lives};
            this.LifeLost?.Invoke(this, lives);

            if (!this.checkForGameOver())
            {
                this.resetTimeRemainingTimer();
            }
        }

        private void handleFrogMadeItHome(HomeFrog frogHome)
        {
            if (frogHome.Sprite.Visibility == Visibility.Visible)
            {
                this.handleLifeLost();
            }
            else
            {
                this.handlePlayerScored();
                frogHome.Sprite.Visibility = Visibility.Visible;
                this.playerStats.IncrementFrogsInHomes();
                this.checkForGameOver();
            }
        }

        private void handleDeathAnimationHasEnded()
        {
            this.deathAnimationManager.CollapseAllAnimationFrames();
            this.deathAnimationTimer.Stop();
            this.deathAnimationManager.ResetFrameCount();
            if (!this.gameIsOver)
            {
                this.player.Sprite.Visibility = Visibility.Visible;
                this.player.EnableMovement();
                this.resetTimeRemainingTimer();
                this.laneManager.ResetLanesToOneVehicle();
            }
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