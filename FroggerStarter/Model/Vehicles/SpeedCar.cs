﻿using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Vehicles;

namespace FroggerStarter.Model.Vehicles
{
    /// <summary>Stores information for the speed car object class.</summary>
    /// <seealso cref="Vehicle" />
    public class SpeedCar : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SpeedCar" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public SpeedCar(double defaultSpeed, Direction direction) : base(defaultSpeed, direction)
        {
            Sprite = new SpeedCarSprite();
        }

        #endregion
    }
}