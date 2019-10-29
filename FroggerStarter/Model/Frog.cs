using FroggerStarter.Constants;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the frog model
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Frog : GameObject
    {
        #region Data members

        private const int SpeedXDirection = GameSettings.PlayerMovementSpeed;
        private const int SpeedYDirection = GameSettings.PlayerMovementSpeed;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Frog" /> class.
        /// </summary>
        public Frog()
        {
            Sprite = new FrogSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the game object right.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveRightWithBoundaryCheck(double rightBoundary)
        {
            if (X + SpeedX < rightBoundary)
            {
                base.MoveRight();
            }
        }

        /// <summary>
        ///     Enables movement.
        ///     Precondition: None
        ///     Postcondition: base.speedX = SpeedXDirection, base.speedY = SpeedYDirection
        /// </summary>
        public void EnableMovement()
        {
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        /// <summary>
        ///     Moves the game object left.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveLeftWithBoundaryCheck(double leftBoundary)
        {
            if (X > leftBoundary)
            {
                base.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the game object up.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev - SpeedY
        /// </summary>
        public void MoveUpWithBoundaryCheck(double topBoundary)
        {
            if (Y > topBoundary)
            {
                MoveUp();
            }
        }

        /// <summary>
        ///     Moves the game object down.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev + SpeedY
        /// </summary>
        public void MoveDownWithBoundaryCheck(double bottomBoundary)
        {
            if (Y + SpeedY < bottomBoundary - LaneSettings.BottomLaneOffset)
            {
                MoveDown();
            }
        }

        #endregion
    }
}