using System;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.DeathAnimation;

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
                    return new FirstDeathAnimationFrame();
                case 2:
                    return new SecondDeathAnimationFrame();
                case 3:
                    return new ThirdDeathAnimationFrame();
                case 4:
                    return new FourthDeathAnimationFrame();
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}