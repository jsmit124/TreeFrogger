using System;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Factory
{
    /// <summary>
    ///     Builds and returns the specified animation sprite
    /// </summary>
    public static class DeathAnimationFactory
    {
        #region Methods

        /// <summary>
        ///     Builds the animation sprite.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="frameNumber">The frame number.</param>
        /// <returns>Returns the specified death animation sprite</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static BaseSprite BuildAnimationSprite(int frameNumber)
        {
            switch (frameNumber)
            {
                case 1:
                    return new DeathAnimationFrame1();
                case 2:
                    return new DeathAnimationFrame2();
                case 3:
                    return new DeathAnimationFrame3();
                case 4:
                    return new DeathAnimationFrame4();
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}