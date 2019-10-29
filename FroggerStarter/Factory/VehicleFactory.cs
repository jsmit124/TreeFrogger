using System;
using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Factory
{
    /// <summary>
    ///     Factory class for building sprites for the Vehicle class
    /// </summary>
    public static class VehicleFactory
    {
        #region Methods

        /// <summary>
        ///     Builds the vehicle sprite.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="typeOfVehicle">The type of vehicle.</param>
        /// <returns>Returns the specified vehicle sprite</returns>
        public static BaseSprite BuildVehicleSprite(VehicleType typeOfVehicle)
        {
            switch (typeOfVehicle)
            {
                case VehicleType.SportsCar:
                    return new CarSprite();
                case VehicleType.Semi:
                    return new SemiSprite();
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}