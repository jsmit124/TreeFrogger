using System;
using FroggerStarter.Enums;
using FroggerStarter.Model.PowerUps;

namespace FroggerStarter.Factory
{
    /// <summary>
    ///     Factory class for building sprites for the Vehicle class
    /// </summary>
    public class PowerUpFactory
    {

        /// <summary>Builds the power up.</summary>
        /// <param name="typeOfPowerUp">The type of power up.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static PowerUp BuildPowerUp(PowerUpType typeOfPowerUp)
        {
            switch (typeOfPowerUp)
            {
                case PowerUpType.Immunity:
                    return new ImmunityPowerUp();
                case PowerUpType.Timer:
                    return new TimerPowerUp();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
