using System;
using FroggerStarter.Constants;
using FroggerStarter.Enums;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>
    ///     Stores information for the Vehicle GameObject class
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public abstract class Vehicle : GameObject
    {
        #region Properties

        /// <summary>Gets the direction.</summary>
        /// <value>The direction.</value>
        public Direction Direction { get; set; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Vehicle" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected Vehicle(double defaultSpeed, Direction direction)
        {
            if (defaultSpeed < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.Direction = direction;
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
        public void MoveForward()
        {
            switch (this.Direction)
            {
                case Direction.Left:
                    this.MoveLeft();
                    break;
                case Direction.Right:
                    this.MoveRight();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Increases the speed.
        ///     Precondition: None.
        ///     PostCondition: SpeedX == SpeedX@prev + speed AND SpeedY == SpeedY@prev + speed
        /// </summary>
        public void IncreaseSpeed(double speed)
        {
            SetSpeed(SpeedX + speed, SpeedY + speed);
        }

        #endregion
    }
}