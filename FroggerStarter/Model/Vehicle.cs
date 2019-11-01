using System;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Factory;

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

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is moving.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is moving; otherwise, <c>false</c>.
        /// </value>
        public bool IsMoving { get; private set; }

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
            if (defaultSpeed < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Sprite = VehicleFactory.BuildVehicleSprite(vehicleType);

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

            if (X > LaneSettings.LaneLength)
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
                X = LaneSettings.LaneLength;
            }
        }

        /// <summary> 
        ///     Moves the vehicle forward.
        ///     Precondition: None
        ///     Postcondition: vehicle is moved forward
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void MoveForward(LaneDirection direction)
        {
            switch (direction)
            {
                case LaneDirection.Left:
                    this.MoveLeft();
                    break;
                case LaneDirection.Right:
                    this.MoveRight();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Stops the movement.
        ///     Precondition: None
        ///     Postcondition: SpeedX and SpeedY set to zero and this.IsMoving = false
        /// </summary>
        public override void StopMovement()
        {
            base.StopMovement();
            this.IsMoving = false;
        }

        /// <summary>
        ///     Starts the vehicle movement.
        ///     Precondition: None
        ///     Postcondition: SpeedX set to this.defaultSpeed and this.IsMoving = true
        /// </summary>
        public void StartMovement()
        {
            SetSpeed(this.defaultSpeed, 0);
            this.IsMoving = true;
        }

        /// <summary>
        ///     Resets this vehicle to no movement and default Y location
        ///     Precondition: None
        ///     Postcondition: this.speed == 0, this.IsMoving = false, base.X = xLocation
        /// </summary>
        public void Reset(double xLocation)
        {
            this.StopMovement();
            X = xLocation;
        }

        #endregion
    }
}