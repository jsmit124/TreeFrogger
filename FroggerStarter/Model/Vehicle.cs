using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;
using System;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the Vehicle GameObject class
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Vehicle : GameObject
    {
        #region Data members

        private readonly double defaultSpeed;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vehicle" /> class.
        ///     Precondition: defaultSpeed > 0
        ///     Postcondition: this.defaultSpeed == defaultSpeed AND base.SpeedX == default speed
        /// </summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        public Vehicle(VehicleType vehicleType, double defaultSpeed)
        {
            if (defaultSpeed <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (vehicleType)
            {
                case VehicleType.SportsCar:
                    Sprite = new CarSprite();
                    break;
                case VehicleType.Semi:
                    Sprite = new SemiSprite();
                    break;
            }

            this.defaultSpeed = defaultSpeed;
            SetSpeed(defaultSpeed, 0);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the game object right.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public override void MoveRight()
        {
            base.MoveRight();

            if (X > Defaults.LaneLength)
            {
                X = 0 - Width;
            }
        }

        /// <summary>
        ///     Moves the game object left.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public override void MoveLeft()
        {
            base.MoveLeft();

            if (X < 0 - Width)
            {
                X = Defaults.LaneLength;
            }
        }

        /// <summary>
        ///     Increments the speed.
        ///     Precondition: amountToIncrement > 0
        ///     Postcondition: base.SpeedX == base.SpeedX + amountToIncrement
        /// </summary>
        /// <param name="amountToIncrement">The amount to increment.</param>
        public void IncrementSpeed(double amountToIncrement)
        {
            if (amountToIncrement <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            SetSpeed(SpeedX + amountToIncrement, 0);
        }

        /// <summary>
        ///     Resets the speed to default.
        ///     Precondition: None
        ///     Postcondition: base.SpeedX == this.defaultSpeed
        /// </summary>
        public void ResetSpeedToDefault()
        {
            SetSpeed(this.defaultSpeed, 0);
        }

        #endregion
    }
}