using FroggerStarter.View.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggerStarter.Factory
{
    /// <summary>
    /// Builds and returns the specified animation sprite
    /// </summary>
    public static class DeathAnimationFactory
    {

        /// <summary>
        /// Builds the animation sprite.
        /// </summary>
        /// <param name="frameNumber">The frame number.</param>
        /// <returns></returns>
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
    }
}
