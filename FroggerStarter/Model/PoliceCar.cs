﻿using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Vehicles;

namespace FroggerStarter.Model
{
    /// <summary>Stores information for the police car object class.</summary>
    public class PoliceCar : Vehicle
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="PoliceCar" /> class.</summary>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        public PoliceCar(double defaultSpeed, LaneDirection direction) : base(defaultSpeed, direction)
        {
            Sprite = new CarSprite();
        }

        #endregion
    }
}