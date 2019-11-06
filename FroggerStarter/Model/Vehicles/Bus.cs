using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Vehicles;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>Stores information for the bus object class.</summary>
    /// <seealso cref="Vehicle" />
    public class Bus : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Bus" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public Bus(double defaultSpeed, Direction direction) : base(defaultSpeed, direction)
        {
            Sprite = new BusSprite();
        }

        #endregion
    }
}