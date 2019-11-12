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
            if (backgroundHeight <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundHeight));

            if (backgroundWidth <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundWidth));

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;

            laneManager = new LaneManager(LaneSettings.MiddleSafeLaneLocation);
            riverManager = new RiverManager(LaneSettings.TopLaneYLocation);
            homeManager = new FrogHomeManager(LaneSettings.TopLaneYLocation);
            deathAnimationManager = new DeathAnimationManager();
            powerUpManager = new PowerUpManager();
            playerManager = new PlayerManager();

            deathAnimationManager.AnimationOver += handleDeathAnimationHasEnded;
        }

        #endregion

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
            gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            addPowerUpsToView();
            addVehiclesToView();
            addLogsToView();
            addDeathAnimationsToView();
            addPlayerMovementAnimationsToView();
            placeHomeFrogs();
            laneManager.HideVehicles();
            createAndPlacePlayer();
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            playerManager.MovePlayer(0, Direction.Left);
        }

        /// <summary>
        ///     Moves the playerSprite to the right.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev + playerSprite.Width
        /// </summary>
        public void MovePlayerRight()
        {
            playerManager.MovePlayer(backgroundWidth, Direction.Right);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp()
        {
            playerManager.MovePlayer(LaneSettings.TopLaneYLocation, Direction.Up);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown()
        {
            playerManager.MovePlayer((int) Math.Floor(backgroundHeight), Direction.Down);
        }

        /// <summary>
        ///     Removes the sprites.
        ///     Precondition: None
        ///     Postcondition: Sprites removed from canvas children
        /// </summary>
        public void RemoveSprites()
        {
            gameCanvas.Children.Remove(playerManager.Sprite);
            removePowerUpSprites();
            removeVehicleSprites();
            removeLogSprites();
            removeHomeFrogSprites();
        }

        private void gameTimerOnTick(object sender, object e)
        {
            laneManager.MoveVehicles();
            riverManager.MoveVehicles();
            checkForPlayerCollisionWithObjects();
            checkForPlayerHitTopWall();
        }

        private void timeRemainingTimerOnTick(object sender, object e)
        {
            playerManager.DecrementTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = playerManager.TimeRemaining};
            TimeRemainingCount?.Invoke(this, timeRemaining);

            if (playerManager.TimeRemaining == 0) handleTimeRemainingIsZero();
        }

        private void checkForPlayerCollisionWithObjects()
        {
            checkForPlayerCollisionWithVehicles();
            checkForPlayerCollisionWithLogs();
            checkForPlayerLeftLog();
            checkForPlayerCollisionWithHome();
            checkForPlayerCollisionWithTimerPowerUp();
            checkForPlayerIsOffScreen();
            checkForDeathAnimationActive();
        }

        private void checkForPlayerCollisionWithVehicles()
        {
            foreach (var vehicle in laneManager)
                if (playerManager.CollisionDetected(vehicle) &&
                    vehicle.Sprite.Visibility != Visibility.Collapsed && !playerIsImmune)
                {
                    DiedHitByVehicle?.Invoke(this, EventArgs.Empty);
                    handleLifeLost();
                }
        }

        private void checkForPlayerIsOffScreen()
        {
            if (playerManager.IsOffScreen())
            {
                DiedHitWall?.Invoke(this, EventArgs.Empty);
                handleLifeLost();
            }
        }

        private void checkForPlayerCollisionWithLogs()
        {
            playerOnMovingLog = false;
            foreach (var log in riverManager)
                if (playerManager.CollisionDetected(log) && playerManager.HasCrossedRoad() &&
                    log.Sprite.Visibility != Visibility.Collapsed)
                    handlePlayerLandedOnLog(log);
        }

        private void checkForPlayerLeftLog()
        {
            if (!playerOnMovingLog && playerManager.HasCrossedRoad())
            {
                DiedInWater?.Invoke(this, EventArgs.Empty);
                handleLifeLost();
            }
        }

        private void checkForPlayerCollisionWithHome()
        {
            foreach (var frogHome in homeManager)
                if (playerManager.CollisionDetectedWithFrogHome(frogHome))
                    handleFrogMadeItHome(frogHome);
        }

        private void checkForPlayerCollisionWithTimerPowerUp()
        {
            foreach (var powerUp in powerUpManager)
                if (playerManager.CollisionDetected(powerUp) && powerUp.Sprite.Visibility != Visibility.Collapsed)
                    handlePowerUpIsHit(powerUp);
        }

        private void checkForLevelOver()
        {
            if (playerManager.AmountOfFrogsInHome != GameSettings.FrogHomeCount) return;

            handleLevelOver();
        }

        private bool checkForGameOver()
        {
            if (playerManager.Lives != 0 && !roundThreeOver) return false;

            handleGameOver();
            return true;
        }

        private void checkForPlayerHitTopWall()
        {
            if (playerManager.Y <= LaneSettings.TopLaneYLocation)
            {
                DiedHitWall?.Invoke(this, EventArgs.Empty);
                handleLifeLost();
            }
        }

        private void checkForDeathAnimationActive()
        {
            if (deathAnimationActive) playerManager.Sprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Starts the game.
        /// </summary>
        public void StartGame()
        {
            setupGameTimer();
            setupTimeRemainingTimer();
            powerUpManager.startPowerUpTimer();
            playerManager.EnableMovement();
        }

        private void handleLevelOver()
        {
            if (!roundOneOver)
            {
                roundOneOver = true;
                enableNextLevel(.5);
            }
            else if (!roundTwoOver)
            {
                roundTwoOver = true;
                enableNextLevel(1);
            }
            else if (!roundThreeOver)
            {
                roundThreeOver = true;
                handleGameOver();
            }

            playerManager.IncrementLevel();
            var level = new LevelIncreasedEventArgs {Level = playerManager.Level};
            LevelIncreased?.Invoke(this, level);
        }

        private void handleLifeLost()
        {
            playerOnMovingLog = false;
            playerManager.Sprite.Visibility = Visibility.Collapsed;
            playerManager.DecrementLives();

            var lives = new LivesLostEventArgs {Lives = playerManager.Lives};
            LifeLost?.Invoke(this, lives);

            timeRemainingTimer.Stop();
            deathAnimationActive = true;
            handleStartDeathAnimation();

            if (!checkForGameOver()) setPlayerToCenterOfBottomLane();
        }

        private void handlePlayerLandedOnLog(Vehicle log)
        {
            playerOnMovingLog = true;
            playerManager.MakePlayerStayOnLog(log);
        }

        private void handleStartDeathAnimation()
        {
            playerManager.DisableMovement();
            playerManager.Sprite.Visibility = Visibility.Collapsed;

            setDeathAnimationToPlayerLocation();
            deathAnimationManager.RotateAllSprites(playerManager.CurrentDirection);
            deathAnimationManager.PlayDeathAnimation();
        }

        private void handlePlayerScored()
        {
            playerOnMovingLog = false;
            playerManager.IncrementScore(playerManager.TimeRemaining * playerManager.Lives);

            var score = new ScoreIncreasedEventArgs {Score = playerManager.Score};
            ScoreIncreased?.Invoke(this, score);

            resetTimeRemainingTimer();
            setPlayerToCenterOfBottomLane();
        }

        private void handleGameOver()
        {
            gameIsOver = true;
            gameTimer.Stop();
            playerManager.Sprite.Visibility = Visibility.Collapsed;
            removePowerUpSprites();
            playerManager.DisableMovement();
            timeRemainingTimer.Stop();
            powerUpManager.stopPowerUpTimer();
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void handleTimeRemainingIsZero()
        {
            DiedTimeRanOut?.Invoke(this, EventArgs.Empty);
            handleLifeLost();

            if (!checkForGameOver()) resetTimeRemainingTimer();
        }

        private void handleFrogMadeItHome(FrogHome frogHome)
        {
            if (frogHome.Sprite.Visibility == Visibility.Visible)
            {
                handleLifeLost();
            }
            else
            {
                playerOnMovingLog = false;
                handlePlayerScored();
                frogHome.Sprite.Visibility = Visibility.Visible;
                playerManager.IncrementFrogsInHomes();
                checkForGameOver();
                checkForLevelOver();
                playerIsImmune = false;
            }
        }

        private void handleDeathAnimationHasEnded(object sender, EventArgs e)
        {
            deathAnimationManager.CollapseAllAnimationFrames();
            deathAnimationManager.StopAnimationTimer();
            deathAnimationManager.ResetFrameCount();

            if (gameIsOver) return;

            playerManager.Sprite.Visibility = Visibility.Visible;
            playerManager.EnableMovement();
            resetTimeRemainingTimer();
            laneManager.HideVehicles();
            deathAnimationActive = false;
        }

        private async void handlePowerUpIsHit(PowerUp powerUp)
        {
            PowerUpActivated?.Invoke(this, EventArgs.Empty);
            if (powerUp.PowerUpType == PowerUpType.Timer)
            {
                playerManager.TimerPowerUp();
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = playerManager.TimeRemaining};
                TimeRemainingCount?.Invoke(this, timeRemaining);
            }
            else
            {
                playerIsImmune = true;
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                await Task.Delay(3000);
                playerIsImmune = false;
            }
        }

        private void createAndPlacePlayer()
        {
            gameCanvas.Children.Add(playerManager.Sprite);
            setPlayerToCenterOfBottomLane();
            playerManager.DisableMovement();
        }

        private void placeHomeFrogs()
        {
            homeManager.ToList().ForEach(homeFrog => gameCanvas.Children.Add(homeFrog.Sprite));
        }

        private void addVehiclesToView()
        {
            laneManager.ToList().ForEach(vehicle => gameCanvas.Children.Add(vehicle.Sprite));
        }

        private void addLogsToView()
        {
            riverManager.ToList().ForEach(vehicle => gameCanvas.Children.Add(vehicle.Sprite));
        }

        private void addDeathAnimationsToView()
        {
            deathAnimationManager.ToList().ForEach(animation => gameCanvas.Children.Add(animation.Sprite));
        }

        private void addPlayerMovementAnimationsToView()
        {
            gameCanvas.Children.Add(playerManager.MovementSprite);
        }

        private void addPowerUpsToView()
        {
            powerUpManager.ToList().ForEach(powerUp => gameCanvas.Children.Add(powerUp.Sprite));
        }

        private void removeHomeFrogSprites()
        {
            homeManager.ToList().ForEach(homeFrog => gameCanvas.Children.Remove(homeFrog.Sprite));
        }

        private void removeVehicleSprites()
        {
            laneManager.ToList().ForEach(vehicle => gameCanvas.Children.Remove(vehicle.Sprite));
        }

        private void removeLogSprites()
        {
            riverManager.ToList().ForEach(vehicle => gameCanvas.Children.Remove(vehicle.Sprite));
        }

        private void removePowerUpSprites()
        {
            powerUpManager.ToList().ForEach(powerUp => gameCanvas.Children.Remove(powerUp.Sprite));
        }

        private void setDeathAnimationToPlayerLocation()
        {
            deathAnimationManager.SetAnimationLocation(playerManager.X, playerManager.Y);
        }

        private void setPlayerToCenterOfBottomLane()
        {
            playerManager.SetPlayerToCenterOfBottomLane();
        }

        private void resetTimeRemainingTimer()
        {
            timeRemainingTimer.Stop();
            playerManager.ResetTimeRemaining();

            var timeRemaining = new TimeRemainingEventArgs {TimeRemaining = playerManager.TimeRemaining};
            TimeRemainingCount?.Invoke(this, timeRemaining);

            timeRemainingTimer.Start();
        }

        private void enableNextLevel(double speed)
        {
            homeManager.makeFrogHomesCollapsed();
            playerManager.ResetFrogsInHome();
            laneManager.IncreaseAllVehicleSpeed(speed);
            laneManager.IncrementMaxAmountOfVehiclesPerLane();
            riverManager.DecrementMaxAmountOfVehiclesPerLane();
            riverManager.IncreaseAllVehicleSpeed(speed);
        }

        #endregion

        #region Timer Setups

        private void setupGameTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += gameTimerOnTick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            gameTimer.Start();
        }

        private void setupTimeRemainingTimer()
        {
            timeRemainingTimer = new DispatcherTimer();
            timeRemainingTimer.Tick += timeRemainingTimerOnTick;
            timeRemainingTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timeRemainingTimer.Start();
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