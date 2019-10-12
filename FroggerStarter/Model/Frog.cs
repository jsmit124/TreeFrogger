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

        private const int SpeedXDirection = 50;
        private const int SpeedYDirection = 50;

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

        public override void MoveRight()
        {
            if (base.X + this.SpeedX < 650)
            {
                base.MoveRight();
            }
        }

        public override void MoveLeft()
        {
            if (base.X > 0)
            {
                base.MoveLeft();
            }
        }

        public override void MoveUp()
        {
            if (base.Y > 55)
            {
                base.MoveUp();
            }
        }

        public override void MoveDown()
        {
            if (base.Y + this.SpeedY < 405)
            {
                base.MoveDown();
            }
        }

        #endregion
    }
}