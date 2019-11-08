using FroggerStarter.Enums;

namespace FroggerStarter.Model.PowerUps
{
    /// <summary>Store information on the power up class</summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public abstract class PowerUp : BaseObject
    {
        #region Properties

        /// <summary>Gets or sets the type of the power up.</summary>
        /// <value>The type of the power up.</value>
        public PowerUpType PowerUpType { get; protected set; }

        #endregion
    }
}