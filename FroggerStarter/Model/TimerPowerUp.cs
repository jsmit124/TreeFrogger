using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information about the timer power up object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class TimerPowerUp : BaseObject
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimerPowerUp" /> class.</summary>
        public TimerPowerUp()
        {
            Sprite = new TimerPowerUpSprite();
        }

        #endregion
    }
}