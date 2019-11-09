using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using FroggerStarter.Model.Animations;
using FroggerStarter.Model.Vehicles;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PlayerMovementAnimation;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the Player Manager class
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        private DispatcherTimer movementTimer;
        private readonly PlayerMovementAnimation movementSprite;
        private readonly PlayerStatistics playerStats;
        private readonly Frog player;
        private int movementFrameCount;

        private Direction currentDirection;
        private double currentBoundary;

        /// <summary>
        /// Gets the movement sprite.
        /// </summary>
        /// <value>
        /// The movement sprite.
        /// </value>
        public BaseSprite MovementSprite => this.movementSprite.Sprite;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the y.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.player.Y;
            set => this.player.Y = value;
        }

        /// <summary>
        ///     Gets the x.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.player.X;
            set => this.player.X = value;
        }

        /// <summary>
        ///     Gets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite => this.player.Sprite;

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score => this.playerStats.Score;

        /// <summary>
        ///     Gets the time remaining.
        /// </summary>
        /// <value>
        ///     The time remaining.
        /// </value>
        public int TimeRemaining => this.playerStats.TimeRemaining;

        /// <summary>
        ///     Gets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives => this.playerStats.Lives;

        /// <summary>
        ///     Gets the amount of frogs in homes.
        /// </summary>
        /// <value>
        ///     The amount of frogs in homes.
        /// </value>
        public int AmountOfFrogsInHome => this.playerStats.AmountOfFrogsInHome;

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public int Level => this.playerStats.Level;


        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerManager" /> class.
        /// </summary>
        public PlayerManager()
        {
            this.setupMovementTimer();
            this.movementSprite = new PlayerMovementAnimation();
            this.playerStats = new PlayerStatistics();
            this.player = new Frog();
            this.movementFrameCount = 1;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the player to center of bottom lane.
        /// </summary>
        public void SetPlayerToCenterOfBottomLane()
        {
            this.player.X = (double) Application.Current.Resources["AppWidth"] / 2 - this.player.Sprite.Width / 2;
            this.player.Y = (double) Application.Current.Resources["PlayerStartYLocation"];

            this.movementSprite.X = this.player.X;
            this.movementSprite.Y = this.player.Y;
        }

        /// <summary>
        ///     Decrements the time remaining.
        /// </summary>
        public void DecrementTimeRemaining()
        {
            this.playerStats.DecrementTimeRemaining();
        }

        /// <summary>
        ///     Decrements the lives.
        /// </summary>
        public void DecrementLives()
        {
            this.playerStats.DecrementLives();
        }

        /// <summary>
        ///     Increments the frogs in homes.
        /// </summary>
        public void IncrementFrogsInHomes()
        {
            this.playerStats.IncrementFrogsInHomes();
        }

        /// <summary>
        ///     Increments the level.
        /// </summary>
        public void IncrementLevel()
        {
            this.playerStats.IncrementLevel();
        }

        /// <summary>
        ///     Resets the time remaining.
        /// </summary>
        public void ResetTimeRemaining()
        {
            this.playerStats.ResetTimeRemaining();
        }

        /// <summary>
        ///     Increments the score.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void IncrementScore(int amount)
        {
            this.playerStats.IncrementScore(amount);
        }

        /// <summary>
        ///     Timers the power up.
        /// </summary>
        public void TimerPowerUp()
        {
            this.playerStats.TimerPowerUp();
        }

        /// <summary>
        /// Moves the player.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        /// <param name="direction">The direction.</param>
        public void MovePlayer(double boundary, Direction direction)
        {
            this.currentBoundary = boundary;
            this.currentDirection = direction;

            this.rotateSprites(direction);
            this.movementTimer.Start();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        private void movePlayerRight(double boundary)
        {


            this.player.MoveRightWithBoundaryCheck(boundary);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        private void movePlayerUp(double boundary)
        {

           
            this.player.MoveUpWithBoundaryCheck(boundary);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        private void movePlayerDown(double boundary)
        {


            this.player.MoveDownWithBoundaryCheck((int) Math.Floor(boundary));
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        private void movePlayerLeft(double boundary)
        {


            this.player.MoveLeftWithBoundaryCheck(boundary);
        }

        private void rotateSprites(Direction direction)
        {
            this.player.RotateSprite(direction);
            this.movementSprite.RotateSprite(direction);
        }

        /// <summary>Makes the player stay on log.</summary>
        /// <param name="log"></param>
        public void MakePlayerStayOnLog(Vehicle log)
        {
            this.player.MoveWithLog(log.Direction, log.SpeedX);
        }

        /// <summary>
        ///     Collisions the detected.
        /// </summary>
        /// <param name="otherObject">The other object.</param>
        /// <returns>True if collision is detected, false otherwise</returns>
        public bool CollisionDetected(BaseObject otherObject)
        {
            return this.player.CollisionDetected(otherObject);
        }

        /// <summary>
        ///     Collisions the detected with frog home.
        /// </summary>
        /// <param name="home">The home.</param>
        /// <returns>True if collision is detected, false otherwise</returns>
        public bool CollisionDetectedWithFrogHome(FrogHome home)
        {
            return this.player.CollisionDetectedWithFrogHome(home);
        }

        /// <summary>
        ///     Enables the player movement.
        /// </summary>
        public void EnableMovement()
        {
            this.player.EnableMovement();
        }

        /// <summary>
        ///     Disables the player movement.
        /// </summary>
        public void DisableMovement()
        {
            this.player.StopMovement();
        }

        /// <summary>
        ///     Resets the frogs in home.
        /// </summary>
        public void ResetFrogsInHome()
        {
            this.playerStats.ResetFrogsInHomes();
        }

        /// <summary>
        ///     Has player crossed the road.
        /// </summary>
        /// <returns>
        ///     True if player crossed road, false otherwise
        /// </returns>
        public bool HasCrossedRoad()
        {
            return this.player.Y < LaneSettings.MiddleSafeLaneLocation &&
                   !this.player.Y.Equals(LaneSettings.TopLaneYLocation);
        }

        /// <summary>Determines whether [is off screen].</summary>
        /// <returns>
        ///   <c>true</c> if [is off screen]; otherwise, <c>false</c>.</returns>
        public bool IsOffScreen()
        {
            return this.player.IsOffScreen();
        }

        private void setupMovementTimer()
        {
            this.movementTimer = new DispatcherTimer();
            this.movementTimer.Tick += this.movementTimerOnTick;
            this.movementTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
        }

        private void moveMovementSprite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.movementSprite.Y = this.player.Y - (this.player.SpeedY / 2);
                    this.movementSprite.X = this.player.X;
                    break;
                case Direction.Down:
                    this.movementSprite.Y = this.player.Y + (this.player.SpeedY / 2);
                    this.movementSprite.X = this.player.X;
                    break;
                case Direction.Left:
                    this.movementSprite.Y = this.player.Y;
                    this.movementSprite.X = this.player.X - (this.player.SpeedX / 2);
                    break;
                case Direction.Right:
                    this.movementSprite.Y = this.player.Y;
                    this.movementSprite.X = this.player.X + (this.player.SpeedX / 2);
                    break;
            }
        }

        private void movementTimerOnTick(object sender, object e)
        {

            if (this.movementFrameCount > 2)
            {
                this.movementTimer.Stop();
                this.movementFrameCount = 0;
            }

            switch (this.movementFrameCount)
            {
                case 1:
                    this.moveMovementSprite(this.currentDirection);
                    this.player.Sprite.Visibility = Visibility.Collapsed;
                    this.movementSprite.Sprite.Visibility = Visibility.Visible;

                    break;
                case 2:
                    switch (this.currentDirection)
                    {
                        case Direction.Up:
                            this.movePlayerUp(this.currentBoundary);
                            break;
                        case Direction.Down:
                            this.movePlayerDown(this.currentBoundary);
                            break;
                        case Direction.Left:
                            this.movePlayerLeft(this.currentBoundary);
                            break;
                        case Direction.Right:
                            this.movePlayerRight(this.currentBoundary);
                            break;
                    }

                    this.movementSprite.Sprite.Visibility = Visibility.Collapsed;
                    this.player.Sprite.Visibility = Visibility.Visible;
                    break;
            }
            this.movementFrameCount++;
        }

        #endregion
    }
}