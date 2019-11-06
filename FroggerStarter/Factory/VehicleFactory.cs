using System;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using FroggerStarter.Model.Vehicles;

namespace FroggerStarter.Factory
{
    /// <summary>
    ///     Factory class for building sprites for the Vehicle class
    /// </summary>
    public static class VehicleFactory
    {
        #region Methods

        /// <summary>
        ///     Builds the vehicle.
        ///     Precondition: speed > 0
        ///     Postcondition: None
        /// </summary>
        /// <param name="typeOfVehicle">The type of vehicle.</param>
        /// <param name="direction">The direction of vehicle.</param>
        /// <param name="speed">The speed of vehicle.</param>
        /// <returns>Returns the specified vehicle sprite</returns>
        public static Vehicle BuildVehicleSprite(VehicleType typeOfVehicle, Direction direction, double speed)
        {
            if (speed < 0)
            {
                throw new ArgumentException("speed cannot be less than 0");
            }

            switch (typeOfVehicle)
            {
                case VehicleType.PoliceCar:
                    return new PoliceCar(speed, direction);
                case VehicleType.Bus:
                    return new Bus(speed, direction);
                case VehicleType.SpeedCar:
                    return new SpeedCar(speed, direction);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}