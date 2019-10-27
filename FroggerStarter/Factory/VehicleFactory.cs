﻿using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Factory
{
    /// <summary>
    /// Factory class for building sprites for the Vehicle class
    /// </summary>
    public static class VehicleFactory
    {
        /// <summary>
        /// Builds the vehicle sprite.
        /// </summary>
        /// <param name="typeOfVehicle">The type of vehicle.</param>
        /// <returns></returns>
        public static BaseSprite BuildVehicleSprite(VehicleType typeOfVehicle)
        {
            switch (typeOfVehicle)
            {
                case VehicleType.SportsCar:
                    return new CarSprite();
                default:
                    return new SemiSprite();
            }
        } 
    }
}
