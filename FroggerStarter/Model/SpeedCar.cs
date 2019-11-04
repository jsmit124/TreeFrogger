using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Vehicles;

namespace FroggerStarter.Model
{
    /// <summary>Stores information for the speed car object class.</summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle" />
    public class SpeedCar : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SpeedCar" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public SpeedCar(double defaultSpeed, LaneDirection direction) : base(defaultSpeed, direction)
        {
            Sprite = new SpeedCarSprite();
        }

        #endregion
    }
}