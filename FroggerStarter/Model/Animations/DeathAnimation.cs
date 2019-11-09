using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.Enums;
using FroggerStarter.Factory;

namespace FroggerStarter.Model.Animations
{
    /// <summary>
    ///     Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class DeathAnimation : BaseAnimation
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

        /// <summary>Rotates the sprite.</summary>
        /// <param name="direction">The direction.</param>
        public void RotateSprite(Direction direction)
        {
            Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            switch (direction)
            {
                case Direction.Down:
                    Sprite.RenderTransform = new CompositeTransform {Rotation = 180};
                    break;
                case Direction.Up:
                    Sprite.RenderTransform = new CompositeTransform {Rotation = 0};
                    break;
                case Direction.Left:
                    Sprite.RenderTransform = new CompositeTransform {Rotation = 270};
                    break;
                case Direction.Right:
                    Sprite.RenderTransform = new CompositeTransform {Rotation = 90};
                    break;
            }
        }

        #endregion
    }
}