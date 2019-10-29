using FroggerStarter.Constants;
using FroggerStarter.Factory;
using System;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class DeathAnimation : GameObject
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathAnimation"/> class.
        /// </summary>
        /// <param name="frameNumber">The frame number.</param>
        /// <param name="speed">The speed.</param>
        /// <exception cref="ArgumentException"></exception>
        public DeathAnimation(int frameNumber)
        {
            if (frameNumber < 1 || frameNumber > 4)
            {
                throw new ArgumentException();
            }

            base.Sprite = DeathAnimationFactory.BuildAnimationSprite(frameNumber);
            base.SetSpeed(GameSettings.PlayerMovementSpeed, GameSettings.PlayerMovementSpeed);
        }

        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetLocation(double x, double y)
        {
            base.X = x;
            base.Y = y;
        }

    }
}
