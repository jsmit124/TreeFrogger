using System;
using System.Drawing;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;
using Point = Windows.Foundation.Point;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for basic objects within the game
    /// </summary>
    public abstract class BaseObject
    {
        #region Data members

        private Point location;

        #endregion

        #region Properties

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

        #endregion

        #region Methods

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

            var collisionArea = new Rectangle((int) otherObject.X, (int) otherObject.Y,
                (int) otherObject.Width, (int) otherObject.Height);
            var currentArea = new Rectangle((int) this.X, (int) this.Y, (int) this.Width, (int) this.Height);

            return currentArea.IntersectsWith(collisionArea);
        }

        /// <summary>Collisions the detected with frog home.</summary>
        /// <param name="home">The home.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool CollisionDetectedWithFrogHome(FrogHome home)
        {
            if (home == null)
            {
                throw new ArgumentNullException();
            }

            return ((this.X - home.X) > (this.Width * .1)) && (home.X - this.X) <= (this.Width * .1) && this.CollisionDetected(home);
        }

        /// <summary>
        ///     Flips the sprite horizontal.
        ///     Precondition: None
        ///     Postcondition: Sprite flipped horizontally
        /// </summary>
        public void FlipSpriteHorizontal()
        {
            this.Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
        }

        #endregion
    }
}