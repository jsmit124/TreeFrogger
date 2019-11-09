using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PlayerMovementAnimation;

namespace FroggerStarter.Model.Animations
{
    /// <summary>
    ///     Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class PlayerMovementAnimation : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerMovementAnimation" /> class.
        /// </summary>
        public PlayerMovementAnimation()
        {
            Sprite = new FirstPlayerMovementFrame {Visibility = Visibility.Collapsed};
        }

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

        /// <summary>
        ///     Moves the with log.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        public void MoveWithLog(Direction direction, double speed)
        {
            if (direction == Direction.Left)
            {
                X -= speed;
            }
            else
            {
                X += speed;
            }
        }

        /// <summary>Determines whether [is off screen].</summary>
        /// <returns>
        ///   <c>true</c> if [is off screen]; otherwise, <c>false</c>.</returns>
        public bool IsOffScreen()
        {
            return (X <= (0 - this.Width) || X >= LaneSettings.LaneLength);
        }

        #endregion
    }
}