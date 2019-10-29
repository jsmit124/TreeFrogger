using FroggerStarter.Constants;
using FroggerStarter.Model;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the playerSprite,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Types and Delegates

        /// <summary>
        ///     Delegate for the Game Over event
        /// </summary>
        public delegate void GameOverHandler();

        /// <summary>
        ///     Delegate for the Lives Decreased event
        /// </summary>
        /// <param name="lives">The lives.</param>
        public delegate void LivesDecreasedHandler(int lives);

        /// <summary>
        ///     Delegate for the Score Increased event
        /// </summary>
        /// <param name="score">The score.</param>
        public delegate void ScoreIncreasedHandler(int score);

        /// <summary>
        /// Delegate for the Timer event
        /// </summary>
        public delegate void TimerHandler(int timeRemaining);

        #endregion

        #region Data members

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private bool gameIsOver = false;
        private Canvas gameCanvas;
        private Frog player;
        private readonly double topLaneYLocation = (Double)Application.Current.Resources["HighRoadYLocation"];
        private readonly PlayerStatistics playerStats;
        private DispatcherTimer gameTimer;
        private DispatcherTimer timeRemainingTimer;
        private DispatcherTimer deathAnimationTimer;
        private readonly LaneManager laneManager;
        private readonly HomeFrogManager homeManager;
        private DeathAnimationManager deathAnimationManager;

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

        /// <summary>
        ///     Occurs when [score increased].
        /// </summary>
        public event ScoreIncreasedHandler ScoreIncreased;

        /// <summary>
        ///     Occurs when [life lost].
        /// </summary>
        public event LivesDecreasedHandler LifeLost;

        /// <summary>
        ///     Occurs when [game over].
        /// </summary>
        public event GameOverHandler GameOver;

        /// <summary>
        /// Occurs when [game timer].
        /// </summary>
        public event TimerHandler TimeRemainingCount;

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

        private void gameTimerOnTick(object sender, object e)
        {
            this.laneManager.MoveVehicles();
            this.checkForPlayerCollisionWithObjects();
            this.checkForPlayerHitTopWall();
        }

        private void timeRemainingTimerOnTick(object sender, object e)
        {
            this.playerStats.DecrementTimeRemaining();
            this.TimeRemainingCount?.Invoke(this.playerStats.TimeRemaining);

            if (this.playerStats.TimeRemaining == 0)
            {
                this.handleTimeRemainingIsZero();
            }
        }

        private void deathAnimationTimerOnTick(object sender, object e)
        {
            if (this.deathAnimationManager.CurrentAnimationFrameIndex > GameSettings.AnimationCount - 1)
            {
                this.deathAnimationManager.CollapseAllAnimationFrames();
                this.deathAnimationTimer.Stop();
                this.deathAnimationManager.ResetFrameCount();
            if (!this.gameIsOver)
            {
                this.player.Sprite.Visibility = Visibility.Visible;
                this.player.EnableMovement();
                this.resetTimeRemainingTimer();
            }
            }
            else
            {
                this.deathAnimationManager.ShowNextFrame();
            }

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
            this.player.MoveDownWithBoundaryCheck((int)Math.Floor(this.backgroundHeight));
        }

        private void checkForPlayerCollisionWithObjects()
        {
            foreach (var vehicle in this.laneManager)
            {
                if (player.CollisionDetected(vehicle))
                {
                    this.handleLifeLost();
                }
            }

            foreach (var frogHome in this.homeManager)
            {
                if (player.CollisionDetected(frogHome))
                {
                    this.handleFrogMadeItHome(frogHome);
                }
            }
        }

        private void handleLifeLost()
        {
            this.player.Sprite.Visibility = Visibility.Collapsed;
            this.playerStats.DecrementLives();
            this.LifeLost?.Invoke(this.playerStats.Lives);

            this.timeRemainingTimer.Stop();
            this.handleDeathAnimation();
            
            if (!this.checkForGameOver())
            {
                this.setPlayerToCenterOfBottomLane();
            }
        }

        private void handleDeathAnimation()
        {
            this.player.StopMovement();
            this.player.Sprite.Visibility = Visibility.Collapsed;

            this.setDeathAnimationToPlayerLocation();
            this.PlayDeathAnimation();
        }

        private void checkForPlayerHitTopWall()
        {
            if (this.player.Y <= this.topLaneYLocation)
            {
                this.handleLifeLost();
            }
        }

        private void handlePlayerScored()
        {
            this.playerStats.IncrementScore(this.playerStats.TimeRemaining);
            this.ScoreIncreased?.Invoke(this.playerStats.Score);
            this.checkForGameOver();
            this.resetTimeRemainingTimer();
            this.setPlayerToCenterOfBottomLane();
        }

        private bool checkForGameOver()
        {
            if (this.playerStats.Lives == 0 || this.playerStats.AmountOfFrogsInHome == GameSettings.FrogHomeCount)
            {
                this.handleGameOver();
                return true;
            }

            return false;
        }

        private void handleGameOver()
        {
            this.gameIsOver = true;
            this.gameTimer.Stop();
            this.player.Sprite.Visibility = Visibility.Collapsed;
            this.player.StopMovement();
            this.timeRemainingTimer.Stop();
            this.laneManager.StopAllVehicleMovement();
            this.GameOver?.Invoke();
        }

        private void handleTimeRemainingIsZero()
        {
            this.playerStats.DecrementLives();
            this.LifeLost?.Invoke(this.playerStats.Lives);
            if (!this.checkForGameOver())
            {
                this.resetTimeRemainingTimer();
            }
        }

        private void resetTimeRemainingTimer()
        {
            this.timeRemainingTimer.Stop();
            this.playerStats.ResetTimeRemaining();
            this.TimeRemainingCount?.Invoke(this.playerStats.TimeRemaining);
            this.timeRemainingTimer.Start();
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

        private void PlayDeathAnimation()
        {

            this.deathAnimationTimer.Start();
        }
        #endregion
    }
}