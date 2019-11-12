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
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerManager" /> class.
        /// </summary>
        public PlayerManager()
        {
            playerStats = new PlayerStatistics();
            player = new Frog();
            createMovementSprite();
        }

        #endregion

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
        public BaseSprite MovementSprite => movementSprite.Sprite;

        /// <summary>
        ///     Gets the y.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => player.Y;
            set => player.Y = value;
        }

        /// <summary>
        ///     Gets the x.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => player.X;
            set => player.X = value;
        }

        /// <summary>
        ///     Gets the sprite.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite => player.Sprite;

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score => playerStats.Score;

        /// <summary>
        ///     Gets the time remaining.
        /// </summary>
        /// <value>
        ///     The time remaining.
        /// </value>
        public int TimeRemaining => playerStats.TimeRemaining;

        /// <summary>
        ///     Gets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives => playerStats.Lives;

        /// <summary>
        ///     Gets the amount of frogs in homes.
        /// </summary>
        /// <value>
        ///     The amount of frogs in homes.
        /// </value>
        public int AmountOfFrogsInHome => playerStats.AmountOfFrogsInHome;

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public int Level => playerStats.Level;

        /// <summary>
        ///     Gets or sets the current direction.
        /// </summary>
        /// <value>
        ///     The current direction.
        /// </value>
        public Direction CurrentDirection { get; private set; }

        #endregion

        #region Methods

        private void createMovementSprite()
        {
            movementSprite = new PlayerMovementAnimation {Sprite = {Visibility = Visibility.Collapsed}};
        }

        /// <summary>
        ///     Sets the player to center of bottom lane.
        /// </summary>
        public void SetPlayerToCenterOfBottomLane()
        {
            X = (double) Application.Current.Resources["AppWidth"] / 2 - player.Sprite.Width / 2;
            Y = (double) Application.Current.Resources["PlayerStartYLocation"];
            movementSprite.X = player.X;
            movementSprite.Y = player.Y;
            checkIfPlayerIsDisabled();
        }

        private void checkIfPlayerIsDisabled()
        {
            if (player.SpeedX.Equals(0))
            {
                Sprite.Visibility = Visibility.Collapsed;
                rotateSprites(Direction.Up);
            }
            else
            {
                Sprite.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        ///     Decrements the time remaining.
        /// </summary>
        public void DecrementTimeRemaining()
        {
            playerStats.DecrementTimeRemaining();
        }

        /// <summary>
        ///     Decrements the lives.
        /// </summary>
        public void DecrementLives()
        {
            playerStats.DecrementLives();
        }

        /// <summary>
        ///     Increments the frogs in homes.
        /// </summary>
        public void IncrementFrogsInHomes()
        {
            playerStats.IncrementFrogsInHomes();
        }

        /// <summary>
        ///     Increments the level.
        /// </summary>
        public void IncrementLevel()
        {
            playerStats.IncrementLevel();
        }

        /// <summary>
        ///     Resets the time remaining.
        /// </summary>
        public void ResetTimeRemaining()
        {
            playerStats.ResetTimeRemaining();
        }

        /// <summary>
        ///     Increments the score.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void IncrementScore(int amount)
        {
            playerStats.IncrementScore(amount);
        }

        /// <summary>
        ///     Timers the power up.
        /// </summary>
        public void TimerPowerUp()
        {
            playerStats.TimerPowerUp();
        }

        /// <summary>
        ///     Moves the player.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        /// <param name="direction">The direction.</param>
        public async void MovePlayer(double boundary, Direction direction)
        {
            CurrentDirection = direction;
            if (player.SpeedX > 0)
                await moveBothSprites(boundary, direction);
            else
                Sprite.Visibility = Visibility.Collapsed;
        }

        private async Task moveBothSprites(double boundary, Direction direction)
        {
            rotateSprites(direction);
            switch (direction)
            {
                case Direction.Up:
                    movePlayerUp(boundary);
                    await Task.Delay(70);
                    moveMovementSprite(direction);
                    break;
                case Direction.Right:
                    movePlayerRight(boundary);
                    await Task.Delay(70);
                    moveMovementSprite(direction);
                    break;
                case Direction.Left:
                    movePlayerLeft(boundary);
                    await Task.Delay(70);
                    moveMovementSprite(direction);
                    break;
                default:
                    movePlayerDown(boundary);
                    await Task.Delay(70);
                    moveMovementSprite(direction);
                    break;
            }
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        /// <param name="boundary">The boundary.</param>
        private void movePlayerRight(double boundary)
        {
            Sprite.Visibility = Visibility.Collapsed;
            player.MoveRightWithBoundaryCheck(boundary);
            movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite up.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev - playerSprite.Height
        /// </summary>
        private void movePlayerUp(double boundary)
        {
            Sprite.Visibility = Visibility.Collapsed;
            player.MoveUpWithBoundaryCheck(boundary);
            movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite down.
        ///     Precondition: none
        ///     Postcondition: playerSprite.Y = playerSprite.Y@prev + playerSprite.Height
        /// </summary>
        private void movePlayerDown(double boundary)
        {
            Sprite.Visibility = Visibility.Collapsed;
            player.MoveDownWithBoundaryCheck((int) Math.Floor(boundary));
            movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Moves the playerSprite to the left.
        ///     Precondition: none
        ///     Postcondition: playerSprite.X = playerSprite.X@prev - playerSprite.Width
        /// </summary>
        private void movePlayerLeft(double boundary)
        {
            Sprite.Visibility = Visibility.Collapsed;
            player.MoveLeftWithBoundaryCheck(boundary);
            movementSprite.Sprite.Visibility = Visibility.Visible;
        }

        private void rotateSprites(Direction direction)
        {
            player.RotateSprite(direction);
            movementSprite.RotateSprite(direction);

            Sprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>Makes the player stay on log.</summary>
        /// <param name="log"></param>
        public void MakePlayerStayOnLog(Vehicle log)
        {
            player.MoveWithLog(log.Direction, log.SpeedX);
            movementSprite.MoveWithLog(log.Direction, log.SpeedX);
        }

        /// <summary>
        ///     Collisions the detected.
        /// </summary>
        /// <param name="otherObject">The other object.</param>
        /// <returns>True if collision is detected, false otherwise</returns>
        public bool CollisionDetected(BaseObject otherObject)
        {
            return player.CollisionDetected(otherObject);
        }

        /// <summary>
        ///     Collisions the detected with frog home.
        /// </summary>
        /// <param name="home">The home.</param>
        /// <returns>True if collision is detected, false otherwise</returns>
        public bool CollisionDetectedWithFrogHome(FrogHome home)
        {
            return player.CollisionDetectedWithFrogHome(home);
        }

        /// <summary>
        ///     Enables the player movement.
        /// </summary>
        public void EnableMovement()
        {
            player.EnableMovement();
            Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Disables the player movement.
        /// </summary>
        public void DisableMovement()
        {
            player.StopMovement();
            Sprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Resets the frogs in home.
        /// </summary>
        public void ResetFrogsInHome()
        {
            playerStats.ResetFrogsInHomes();
        }

        /// <summary>
        ///     Has player crossed the road.
        /// </summary>
        /// <returns>
        ///     True if player crossed road, false otherwise
        /// </returns>
        public bool HasCrossedRoad()
        {
            return player.Y < LaneSettings.MiddleSafeLaneLocation &&
                   !player.Y.Equals(LaneSettings.TopLaneYLocation);
        }

        /// <summary>Determines whether [is off screen].</summary>
        /// <returns>
        ///     <c>true</c> if [is off screen]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOffScreen()
        {
            return player.IsOffScreen();
        }

        private void moveMovementSprite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    movementSprite.Y = player.Y - player.SpeedY / 2;
                    movementSprite.X = player.X;
                    break;
                case Direction.Down:
                    movementSprite.Y = player.Y + player.SpeedY / 2;
                    movementSprite.X = player.X;
                    break;
                case Direction.Left:
                    movementSprite.Y = player.Y;
                    movementSprite.X = player.X - player.SpeedX / 2;
                    break;
                case Direction.Right:
                    movementSprite.Y = player.Y;
                    movementSprite.X = player.X + player.SpeedX / 2;
                    break;
            }

            movementSprite.Sprite.Visibility = Visibility.Collapsed;
            Sprite.Visibility = Visibility.Visible;
        }

        #endregion
    }
}