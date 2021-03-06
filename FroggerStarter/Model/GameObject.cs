﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.Enums;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines basic properties and behavior of every game object.
    /// </summary>
    public abstract class GameObject : BaseObject
    {
        #region Properties

        /// <summary>
        ///     Gets the x speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed x.
        /// </value>
        public double SpeedX { get; private set; }

        /// <summary>
        ///     Gets the y speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed y.
        /// </value>
        public double SpeedY { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the game object right.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public virtual void MoveRight()
        {
            this.moveX(this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object left.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public virtual void MoveLeft()
        {
            this.moveX(-this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object up.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev - SpeedY
        /// </summary>
        public void MoveUp()
        {
            this.moveY(-this.SpeedY);
        }

        /// <summary>
        ///     Moves the game object down.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev + SpeedY
        /// </summary>
        public void MoveDown()
        {
            this.moveY(this.SpeedY);
        }

        private void moveX(double x)
        {
            X += x;
        }

        private void moveY(double y)
        {
            Y += y;
        }

        /// <summary>
        ///     Sets the speed of the game object.
        ///     Precondition: speedX >= 0 AND speedY >=0
        ///     Postcondition: SpeedX == speedX AND SpeedY == speedY
        /// </summary>
        /// <param name="speedX">The speed x.</param>
        /// <param name="speedY">The speed y.</param>
        protected void SetSpeed(double speedX, double speedY)
        {
            if (speedX < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedX));
            }

            if (speedY < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedY));
            }

            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        /// <summary>
        ///     Stops the movement.
        ///     Precondition: None
        ///     Postcondition: SpeedX and SpeedY set to zero
        /// </summary>
        public virtual void StopMovement()
        {
            this.SetSpeed(0, 0);
        }

        /// <summary>
        ///     Rotates the sprite.
        ///     @Precondition: None
        ///     @Postcondition: Sprite is rotated
        /// </summary>
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