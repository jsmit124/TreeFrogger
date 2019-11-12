using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using FroggerStarter.Model.PowerUps;
using FroggerStarter.Model.Vehicles;

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
        private bool playerIsImmune;
        private bool playerOnMovingLog;
        private bool deathAnimationActive;

        private Canvas gameCanvas;

        private readonly LaneManager laneManager;
        private readonly RiverManager riverManager;
        private readonly FrogHomeManager homeManager;
        private readonly DeathAnimationManager deathAnimationManager;
        private readonly PowerUpManager powerUpManager;
        private readonly PlayerManager playerManager;

        private DispatcherTimer gameTimer;
        private DispatcherTimer timeRemainingTimer;

        #endregion

        #region Properties

        /// <summary>Gets the score.</summary>
        /// <value>The score.</value>
        public int Score => this.playerManager.Score;

        /// <summary>Gets the level.</summary>
        /// <value>The level.</value>
        public int Level => this.playerManager.Level;

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

            this.laneManager = new LaneManager(LaneSettings.MiddleSafeLaneLocation);
            this.riverManager = new RiverManager(LaneSettings.TopLaneYLocation);
            this.homeManager = new FrogHomeManager(LaneSettings.TopLaneYLocation);
            this.deathAnimationManager = new DeathAnimationManager();
            this.powerUpManager = new PowerUpManager();
            this.playerManager = new PlayerManager();

            this.deathAnimationManager.AnimationOver += this.handleDeathAnimationHasEnded;
        }

        #endregion

        #region Methods

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
            this.addPowerUpsToView();
            this.addVehiclesToView();
            this.addLogsToView();
            this.addDeathAnimationsToView();
            this.addPlayerMovementAnimationsToView();
            this.placeHomeFrogs();
            this.laneManager.HideVehicles();
            this.createAndPlacePlayer();
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MovePlayer(0, Direction.Left);
        }

        /// <summary>
        ///     Moves the playerSprite to the right.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev + playerSprite.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MovePlayer(this.backgroundWidth, Direction.Right);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.playerManager.MovePlayer(LaneSettings.TopLaneYLocation, Direction.Up);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.playerManager.MovePlayer((int) Math.Floor(this.backgroundHeight), Direction.Down);
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
            this.removeLogSprites();
            this.removeHomeFrogSprites();
        }

        private void gameTimerOnTick(object sender, object e)
        {
            this.laneManager.MoveVehicles();
            this.riverManager.MoveVehicles();
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

        private void checkForPlayerCollisionWithObjects()
        {
            this.checkForPlayerCollisionWithVehicles();
            this.checkForPlayerCollisionWithLogs();
            this.checkForPlayerLeftLog();
            this.checkForPlayerCollisionWithHome();
            this.checkForPlayerCollisionWithTimerPowerUp();
            this.checkForPlayerIsOffScreen();
            this.checkForDeathAnimationActive();
        }

        private void checkForPlayerCollisionWithVehicles()
        {
            foreach (var vehicle in this.laneManager)
            {
                if (this.playerManager.CollisionDetected(vehicle) &&
                    vehicle.Sprite.Visibility != Visibility.Collapsed && !this.playerIsImmune)
                {
                    this.DiedHitByVehicle?.Invoke(this, EventArgs.Empty);
                    this.handleLifeLost();
                }
            }
        }

        private void checkForPlayerIsOffScreen()
        {
            if (this.playerManager.IsOffScreen())
            {
                this.DiedHitWall?.Invoke(this, EventArgs.Empty);
                this.handleLifeLost();
            }
        }

        private void checkForPlayerCollisionWithLogs()
        {
            this.playerOnMovingLog = false;
            foreach (var log in this.riverManager)
            {
                if (this.playerManager.CollisionDetected(log) && this.playerManager.HasCrossedRoad() &&
                    log.Sprite.Visibility != Visibility.Collapsed)
                {
                    this.handlePlayerLandedOnLog(log);
                }
            }
        }

        private void checkForPlayerLeftLog()
        {
            if (!this.playerOnMovingLog && this.playerManager.HasCrossedRoad())
            {
                this.DiedInWater?.Invoke(this, EventArgs.Empty);
                this.handleLifeLost();
            }
        }

        private void checkForPlayerCollisionWithHome()
        {
            foreach (var frogHome in this.homeManager)
            {
                if (this.playerManager.CollisionDetectedWithFrogHome(frogHome))
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
                    this.handlePowerUpIsHit(powerUp);
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
            if (this.playerManager.Y <= LaneSettings.TopLaneYLocation)
            {
                this.DiedHitWall?.Invoke(this, EventArgs.Empty);
                this.handleLifeLost();
            }
        }

        private void checkForDeathAnimationActive()
        {
            if (this.deathAnimationActive)
            {
                this.playerManager.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Starts the game.
        /// </summary>
        public void StartGame()
        {
            this.setupGameTimer();
            this.setupTimeRemainingTimer();
            this.powerUpManager.startPowerUpTimer();
            this.playerManager.EnableMovement();
        }

        private void handleLevelOver()
        {
            if (!this.roundOneOver)
            {
                this.roundOneOver = true;
                this.enableNextLevel(.5);
            }
            else if (!this.roundTwoOver)
            {
                this.roundTwoOver = true;
                this.enableNextLevel(1);
            }
            else if (!this.roundThreeOver)
            {
                this.roundThreeOver = true;
                this.handleGameOver();
            }

            this.playerManager.IncrementLevel();
            var level = new LevelIncreasedEventArgs {Level = this.playerManager.Level};
            this.LevelIncreased?.Invoke(this, level);
        }

        private void handleLifeLost()
        {
            this.playerOnMovingLog = false;
            this.playerManager.Sprite.Visibility = Visibility.Collapsed;
            this.playerManager.DecrementLives();

            var lives = new LivesLostEventArgs {Lives = this.playerManager.Lives};
            this.LifeLost?.Invoke(this, lives);

            this.timeRemainingTimer.Stop();
            this.deathAnimationActive = true;
            this.handleStartDeathAnimation();

            if (!this.checkForGameOver())
            {
                this.setPlayerToCenterOfBottomLane();
            }
        }

        private void handlePlayerLandedOnLog(Vehicle log)
        {
            this.playerOnMovingLog = true;
            this.playerManager.MakePlayerStayOnLog(log);
        }

        private void handleStartDeathAnimation()
        {
            this.playerManager.DisableMovement();
            this.playerManager.Sprite.Visibility = Visibility.Collapsed;

            this.setDeathAnimationToPlayerLocation();
            this.deathAnimationManager.RotateAllSprites(this.playerManager.CurrentDirection);
            this.deathAnimationManager.PlayDeathAnimation();
        }

        private void handlePlayerScored()
        {
            this.playerOnMovingLog = false;
            this.playerManager.IncrementScore(this.playerManager.TimeRemaining * this.playerManager.Lives);

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
            this.DiedTimeRanOut?.Invoke(this, EventArgs.Empty);
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
                this.playerOnMovingLog = false;
                this.handlePlayerScored();
                frogHome.Sprite.Visibility = Visibility.Visible;
                this.playerManager.IncrementFrogsInHomes();
                this.checkForGameOver();
                this.checkForLevelOver();
                this.playerIsImmune = false;
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
            this.deathAnimationActive = false;
        }

        private async void handlePowerUpIsHit(PowerUp powerUp)
        {
            this.PowerUpActivated?.Invoke(this, EventArgs.Empty);
            if (powerUp.PowerUpType == PowerUpType.Timer)
            {
                this.playerManager.TimerPowerUp();
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = this.playerManager.TimeRemaining};
                this.TimeRemainingCount?.Invoke(this, timeRemaining);
            }
            else
            {
                this.playerIsImmune = true;
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                await Task.Delay(3000);
                this.playerIsImmune = false;
            }
        }

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

        private void addLogsToView()
        {
            this.riverManager.ToList().ForEach(vehicle => this.gameCanvas.Children.Add(vehicle.Sprite));
        }

        private void addDeathAnimationsToView()
        {
            this.deathAnimationManager.ToList().ForEach(animation => this.gameCanvas.Children.Add(animation.Sprite));
        }

        private void addPlayerMovementAnimationsToView()
        {
            this.gameCanvas.Children.Add(this.playerManager.MovementSprite);
        }

        private void addPowerUpsToView()
        {
            this.powerUpManager.ToList().ForEach(powerUp => this.gameCanvas.Children.Add(powerUp.Sprite));
        }

        private void removeHomeFrogSprites()
        {
            this.homeManager.ToList().ForEach(homeFrog => this.gameCanvas.Children.Remove(homeFrog.Sprite));
        }

        private void removeVehicleSprites()
        {
            this.laneManager.ToList().ForEach(vehicle => this.gameCanvas.Children.Remove(vehicle.Sprite));
        }

        private void removeLogSprites()
        {
            this.riverManager.ToList().ForEach(vehicle => this.gameCanvas.Children.Remove(vehicle.Sprite));
        }

        private void removePowerUpSprites()
        {
            this.powerUpManager.ToList().ForEach(powerUp => this.gameCanvas.Children.Remove(powerUp.Sprite));
        }

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

        private void enableNextLevel(double speed)
        {
            this.homeManager.makeFrogHomesCollapsed();
            this.playerManager.ResetFrogsInHome();
            this.laneManager.IncreaseAllVehicleSpeed(speed);
            this.laneManager.IncrementMaxAmountOfVehiclesPerLane();
            this.riverManager.DecrementMaxAmountOfVehiclesPerLane();
            this.riverManager.IncreaseAllVehicleSpeed(speed);
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
        ///     The power up activated event handler
        /// </summary>
        public EventHandler<EventArgs> PowerUpActivated;

        /// <summary>
        ///     The player died in water event handler
        /// </summary>
        public EventHandler<EventArgs> DiedInWater;

        /// <summary>
        ///     The player died by getting hit by vehicle event handler
        /// </summary>
        public EventHandler<EventArgs> DiedHitByVehicle;

        /// <summary>
        ///     The player died due to time ran out event handler
        /// </summary>
        public EventHandler<EventArgs> DiedTimeRanOut;

        /// <summary>
        ///     The player died due to hit wall event handler
        /// </summary>
        public EventHandler<EventArgs> DiedHitWall;

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