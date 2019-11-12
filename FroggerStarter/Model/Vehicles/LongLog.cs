using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.RiverLogs;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>Stores information about the long log class.</summary>
    /// <seealso cref="FroggerStarter.Model.Vehicles.Vehicle" />
    public class LongLog : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="LongLog" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public LongLog(double defaultSpeed, Direction direction) : base(defaultSpeed, direction)
        {
            Sprite = new LongLogSprite();
        }

        #endregion
    }
}