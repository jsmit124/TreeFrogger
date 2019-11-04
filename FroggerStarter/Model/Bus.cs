using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Vehicles;

namespace FroggerStarter.Model
{
    /// <summary>Stores information for the bus object class.</summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle" />
    public class Bus : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Bus" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public Bus(double defaultSpeed, LaneDirection direction) : base(defaultSpeed, direction)
        {
            Sprite = new BusSprite();
        }

        #endregion
    }
}