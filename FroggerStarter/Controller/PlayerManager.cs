using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using FroggerStarter.Model.Animations;
using FroggerStarter.Model.Vehicles;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the Player Manager class
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        private PlayerMovementAnimation movementSprite;
        private readonly PlayerStatistics playerStats;
        private readonly Frog player;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the movement sprite.
        /// </summary>
        /// <value>
        ///     The movement sprite.
        /// </value>
        public BaseSprite MovementSprite => this.movementSprite.Sprite;

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
            this.createMovementSprite();
        }

        #endregion

        #region Methods

        private void createMovementSprite()
        {
            this.movementSprite = new PlayerMovementAnimation {Sprite = {Visibility = Visibility.Collapsed}};
        }

        /// <summary>
        ///     Sets the player to center of bottom lane.
        /// </summary>
        public void SetPlayerToCenterOfBottomLane()
        {
            this.X = (double) Application.Current.Resources["AppWidth"] / 2 - this.player.Sprite.Width / 2;
            this.Y = (double) Application.Current.Resources["PlayerStartYLocation"];
            this.movementSprite.X = this.player.X;
            this.movementSprite.Y = this.player.Y;
            this.checkIfPlayerIsDisabled();
        }

        private void checkIfPlayerIsDisabled()
        {
            if (this.player.SpeedX.Equals(0))
            {
                this.Sprite.Visibility = Visibility.Collapsed;
                this.rotateSprites(Direction.Up);
            }
            else
            {
                this.Sprite.Visibility = Visibility.Visible;
            }
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
        ///     Moves the player.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        /// <param name="direction">The direction.</param>
        public async void MovePlayer(double boundary, Direction direction)
        {
            if (this.player.SpeedX > 0)
            {
                await this.moveBothSprites(boundary, direction);
            }
            else
            {
                this.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        private async Task moveBothSprites(double boundary, Direction direction)
        {
            this.rotateSprites(direction);
            switch (direction)
            {
                case Direction.Up:
                    this.movePlayerUp(boundary);
                    await Task.Delay(70);
                    this.moveMovementSprite(direction);
                    break;
                case Direction.Right:
                    this.movePlayerRight(boundary);
                    await Task.Delay(70);
                    this.moveMovementSprite(direction);
                    break;
                case Direction.Left:
                    this.movePlayerLeft(boundary);
                    await Task.Delay(70);
                    this.moveMovementSprite(direction);
                    break;
                default:
                    this.movePlayerDown(boundary);
                    await Task.Delay(70);
                    this.moveMovementSprite(direction);
                    break;
            }
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        private void movePlayerRight(double boundary)
        {
            this.Sprite.Visibility = Visibility.Collapsed;
            this.player.MoveRightWithBoundaryCheck(boundary);
            this.movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        private void movePlayerUp(double boundary)
        {
            this.Sprite.Visibility = Visibility.Collapsed;
            this.player.MoveUpWithBoundaryCheck(boundary);
            this.movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        private void movePlayerDown(double boundary)
        {
            this.Sprite.Visibility = Visibility.Collapsed;
            this.player.MoveDownWithBoundaryCheck((int) Math.Floor(boundary));
            this.movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        private void movePlayerLeft(double boundary)
        {
            this.Sprite.Visibility = Visibility.Collapsed;
            this.player.MoveLeftWithBoundaryCheck(boundary);
            this.movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        private void rotateSprites(Direction direction)
        {
            this.player.RotateSprite(direction);
            this.movementSprite.RotateSprite(direction);

            this.Sprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>Makes the player stay on log.</summary>
        /// <param name="log"></param>
        public void MakePlayerStayOnLog(Vehicle log)
        {
            this.player.MoveWithLog(log.Direction, log.SpeedX);
            this.movementSprite.MoveWithLog(log.Direction, log.SpeedX);
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
            this.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Disables the player movement.
        /// </summary>
        public void DisableMovement()
        {
            this.player.StopMovement();
            this.Sprite.Visibility = Visibility.Collapsed;
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
        ///     <c>true</c> if [is off screen]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOffScreen()
        {
            return this.player.IsOffScreen();
        }

        private void moveMovementSprite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.movementSprite.Y = this.player.Y - this.player.SpeedY / 2;
                    this.movementSprite.X = this.player.X;
                    break;
                case Direction.Down:
                    this.movementSprite.Y = this.player.Y + this.player.SpeedY / 2;
                    this.movementSprite.X = this.player.X;
                    break;
                case Direction.Left:
                    this.movementSprite.Y = this.player.Y;
                    this.movementSprite.X = this.player.X - this.player.SpeedX / 2;
                    break;
                case Direction.Right:
                    this.movementSprite.Y = this.player.Y;
                    this.movementSprite.X = this.player.X + this.player.SpeedX / 2;
                    break;
            }

            this.movementSprite.Sprite.Visibility = Visibility.Collapsed;
            this.Sprite.Visibility = Visibility.Visible;
        }

        #endregion
    }
}