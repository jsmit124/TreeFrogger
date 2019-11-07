using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.PowerUps;

namespace FroggerStarter.Model.PowerUps
{
    /// <summary>
    ///     Stores information about the ImmunityPowerUp object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class ImmunityPowerUp : PowerUp
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ImmunityPowerUp" /> class.</summary>
        public ImmunityPowerUp()
        {
            Sprite = new ImmunityPowerUpSprite();
            PowerUpType = PowerUpType.Immunity;
        }

        #endregion
    }
}