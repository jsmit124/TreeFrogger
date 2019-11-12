using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.PowerUps;

namespace FroggerStarter.Model.PowerUps
{
    /// <summary>
    ///     Stores information about the timer power up object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class TimerPowerUp : PowerUp
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimerPowerUp" /> class.</summary>
        public TimerPowerUp()
        {
            Sprite = new TimerPowerUpSprite();
            PowerUpType = PowerUpType.Timer;
        }

        #endregion
    }
}