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

        public delegate void ScoreIncreasedHandler(int score);

        public delegate void LivesDecreasedHandler(int lives);

        public delegate void GameOverHandler();

        public event ScoreIncreasedHandler ScoreIncreased;
        public event LivesDecreasedHandler LifeLost;
        public event GameOverHandler GameOver;

        private Canvas gameCanvas;
        private Frog playerSprite;
        public PlayerStatistics PlayerStats { get; private set; }

        private DispatcherTimer gameTimer;
        private DispatcherTimer speedTimer;
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
            this.laneManager = new LaneManager();
            this.PlayerStats = new PlayerStatistics();

            this.setupGameTimer();
            this.setupSpeedTimer();
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

        private void setupSpeedTimer()
        {
            this.speedTimer = new DispatcherTimer();
            this.speedTimer.Tick += this.speedTimerOnTick;
            this.speedTimer.Interval = new TimeSpan(0, 0, 1);
            this.speedTimer.Start();
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
            this.playerSprite = new Frog();
            this.gameCanvas.Children.Add(this.playerSprite.Sprite);
            this.setPlayerToCenterOfBottomLane();
        }

        private void addVehiclesToView()
        {
            foreach (var vehicle in this.laneManager.AllVehicles)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.playerSprite.X = this.backgroundWidth / 2 - this.playerSprite.Width / 2;
            this.playerSprite.Y = this.backgroundHeight - this.playerSprite.Height - Defaults.BottomLaneOffset;
        }

        private void gameTimerOnTick(object sender, object e)
        {
            this.laneManager.MoveVehicles();
            this.checkForCollision();
            this.checkForPlayerScored();
        }

        private void speedTimerOnTick(object sender, object e)
        {
            this.laneManager.IncrementSpeed(0.1);
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerSprite.MoveLeft();
        }

        /// <summary>
        ///     Moves the playerSprite to the right.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev + playerSprite.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerSprite.MoveRight();
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.playerSprite.MoveUp();
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.playerSprite.MoveDown();
        }

        private void checkForCollision()
        {
            foreach (var vehicle in this.laneManager.AllVehicles)
            {
                if (vehicle.CollisionDetected(this.playerSprite))
                {
                    this.handleCollision();
                }
            }
        }

        private void handleCollision()
        {
            this.laneManager.ResetVehicleSpeedsToDefault();
            this.PlayerStats.DecrementLives();
            this.LifeLost?.Invoke(this.PlayerStats.Lives);

            if (!this.checkForGameOver())
            {
                this.setPlayerToCenterOfBottomLane();
            }
        }

        private void checkForPlayerScored()
        {
            if (this.playerSprite.Y <= Defaults.TopLaneYLocation)
            {
                this.handlePlayerScored();
            }
        }

        private void handlePlayerScored()
        {
            this.ScoreIncreased?.Invoke(this.PlayerStats.Score);
            this.PlayerStats.IncrementScore();
            this.checkForGameOver();
            this.setPlayerToCenterOfBottomLane();
        }

        private bool checkForGameOver()
        {
            if (this.PlayerStats.Lives == 0 || this.PlayerStats.Score == 3)
            {
                this.handleGameOver();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void handleGameOver()
        {
            this.gameTimer.Stop();
            this.speedTimer.Stop();
            this.playerSprite.StopMovement();
            this.laneManager.StopAllVehicleMovement();
            this.GameOver?.Invoke();
        }

        #endregion
    }
}