using System;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the Player Manager class
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        private readonly PlayerStatistics playerStats;
        private readonly Frog player;

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
            this.playerStats = new PlayerStatistics();
            this.player = new Frog();
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
        ///     Moves the player right.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        public void MovePlayerRight(double boundary)
        {
            this.player.RotateSprite(Direction.Right);
            this.player.MoveRightWithBoundaryCheck(boundary);
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        public void MovePlayerUp(double boundary)
        {
            this.player.RotateSprite(Direction.Up);
            this.player.MoveUpWithBoundaryCheck(boundary);
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        public void MovePlayerDown(double boundary)
        {
            this.player.RotateSprite(Direction.Down);
            this.player.MoveDownWithBoundaryCheck((int) Math.Floor(boundary));
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        public void MovePlayerLeft(double boundary)
        {
            this.player.RotateSprite(Direction.Left);
            this.player.MoveLeftWithBoundaryCheck(boundary);
        }

        /// <summary>Makes the player stay on log.</summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        public void MakePlayerStayOnLog(Direction direction, double speed)
        {
            this.player.MoveWithLog(direction, speed);
        }

        /// <summary>
        ///     Collisions the detected.
        /// </summary>
        /// <param name="otherObject">The other object.</param>
        /// <returns>True is collision is detected, false otherwise</returns>
        public bool CollisionDetected(BaseObject otherObject)
        {
            return this.player.CollisionDetected(otherObject);
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

        #endregion
    }
}