using System;
using FroggerStarter.Factory;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class DeathAnimation : BaseObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeathAnimation" /> class.
        /// </summary>
        /// <param name="frameNumber">The frame number.</param>
        /// <exception cref="ArgumentException"></exception>
        public DeathAnimation(int frameNumber)
        {
            if (frameNumber < 1 || frameNumber > 4)
            {
                throw new ArgumentException();
            }

            Sprite = DeathAnimationFactory.BuildAnimationSprite(frameNumber);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the location.
        ///     Precondition: None
        ///     Postcondition: base.X = x, base.Y = y
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetLocation(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }
}