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

        #endregion

        #region Data members

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private Canvas gameCanvas;
        private Frog player;

        private readonly double topLaneYLocation = (Double)Application.Current.Resources["HighRoadYLocation"];

        private readonly PlayerStatistics playerStats;

        private DispatcherTimer gameTimer;
        private readonly LaneManager laneManager;

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
            this.playerStats = new PlayerStatistics();

            this.setupGameTimer();
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

        private void setupGameTimer()
        {
            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Tick += this.gameTimerOnTick;
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.gameTimer.Start();
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
        }

        private void createAndPlacePlayer()
        {
            this.player = new Frog();
            this.gameCanvas.Children.Add(this.player.Sprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void addVehiclesToView()
        {
            foreach (var vehicle in this.laneManager)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
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
            this.checkForPlayerCollisionWithVehicle();
            this.checkForPlayerScored();
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

        private void checkForPlayerCollisionWithVehicle()
        {
            foreach (var vehicle in this.laneManager)
            {
                if (vehicle.CollisionDetected(this.player))
                {
                    this.handleCollision();
                }
            }
        }

        private void handleCollision()
        {
            this.laneManager.ResetVehicleSpeedsToDefault();
            this.playerStats.DecrementLives();
            this.LifeLost?.Invoke(this.playerStats.Lives);

            if (!this.checkForGameOver())
            {
                this.setPlayerToCenterOfBottomLane();
            }
        }

        private void checkForPlayerScored()
        {
            if (this.player.Y <= this.topLaneYLocation)
            {
                this.handlePlayerScored();
            }
        }

        private void handlePlayerScored()
        {
            this.ScoreIncreased?.Invoke(this.playerStats.Score);
            this.playerStats.IncrementScore();
            this.checkForGameOver();
            this.setPlayerToCenterOfBottomLane();
        }

        private bool checkForGameOver()
        {
            if (this.playerStats.Lives == 0 || this.playerStats.Score == GameSettings.WinningScore)
            {
                this.handleGameOver();
                return true;
            }

            return false;
        }

        private void handleGameOver()
        {
            this.gameTimer.Stop();
            this.laneManager.StopSpeedTimer();
            this.player.StopMovement();
            this.laneManager.StopAllVehicleMovement();
            this.GameOver?.Invoke();
        }

        #endregion
    }
}