using FroggerStarter.View.Sprites;
using System;
using System.Drawing;
using Windows.UI.Xaml.Media;
using Point = Windows.Foundation.Point;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores information for basic objects within the game
    /// </summary>
    public abstract class BaseObject
    {
        private Point location;

        /// <summary>
        ///     Gets or sets the x location of the game object.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.location.X;
            set
            {
                this.location.X = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets or sets the y location of the game object.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.location.Y;
            set
            {
                this.location.Y = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets the width of the game object.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height of the game object.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets the sprite associated with the game object.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; protected set; }

        private void render()
        {
            this.Sprite.RenderAt(this.X, this.Y);
        }

        /// <summary>
        ///     Detects collisions of two game objects.
        ///     Precondition: otherObject != null
        ///     Postcondition: None
        /// </summary>
        /// <param name="otherObject">The other object.</param>
        /// <returns></returns>
        public bool CollisionDetected(BaseObject otherObject)
        {
            if (otherObject == null)
            {
                throw new ArgumentNullException();
            }

            var collisionArea = new Rectangle((int)otherObject.X, (int)otherObject.Y,
                (int)otherObject.Width, (int)otherObject.Height);
            var currentArea = new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);

            return currentArea.IntersectsWith(collisionArea);
        }

        /// <summary>
        ///     Flips the sprite horizontal.
        ///     Precondition: None
        ///     Postcondition: Sprite flipped horizontally
        /// </summary>
        public void FlipSpriteHorizontal()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform { ScaleX = -1 };
        }
    }
}
