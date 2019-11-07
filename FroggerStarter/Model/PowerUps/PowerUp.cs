using FroggerStarter.Enums;

namespace FroggerStarter.Model.PowerUps
{
    /// <summary>Store information on the power up class</summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public abstract class PowerUp : BaseObject
    {
        /// <summary>Gets or sets the type of the power up.</summary>
        /// <value>The type of the power up.</value>
        public PowerUpType PowerUpType { get; protected set; }

        /// <summary>Initializes a new instance of the <see cref="PowerUp"/> class.</summary>
        /// <param name="type">The type.</param>
        protected PowerUp()
        {
        }
    }
}