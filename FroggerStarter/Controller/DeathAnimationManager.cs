using FroggerStarter.Constants;
using FroggerStarter.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Stores information for the default animation manager class
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.DeathAnimation}" />
    public class DeathAnimationManager : IEnumerable<DeathAnimation>
    {
        private IList<DeathAnimation> animations;
        /// <summary>
        /// Gets the index of the current animation frame.
        /// </summary>
        /// <value>
        /// The index of the current animation frame.
        /// </value>
        public int CurrentAnimationFrameIndex { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathAnimationManager"/> class.
        /// </summary>
        public DeathAnimationManager()
        {
            this.animations = new List<DeathAnimation>();
            this.buildAnimationCollection();
            this.ResetFrameCount();
            this.CollapseAllAnimationFrames();
        }

        /// <summary>
        /// Resets the frame count.
        /// </summary>
        public void ResetFrameCount()
        {
            this.CurrentAnimationFrameIndex = 0;
        }

        private void buildAnimationCollection()
        {
            var count = 1;
            while (count <= GameSettings.AnimationCount)
            {
                this.animations.Add(new DeathAnimation(count));
                count++;
            }
        }

        /// <summary>
        /// Sets the animation location.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetAnimationLocation(double x, double y)
        {
            foreach (var animation in this.animations)
            {
                animation.SetLocation(x, y);
            }
        }

        /// <summary>
        /// Shows the next death animation frame.
        /// </summary>
        public void ShowNextFrame()
        {
            if (this.CurrentAnimationFrameIndex > 0)
            {
                var previousFrame = this.animations[CurrentAnimationFrameIndex];
                previousFrame.Sprite.Visibility = Visibility.Collapsed;
            }

            if (this.CurrentAnimationFrameIndex <= GameSettings.AnimationCount - 1)
            {
                var nextFrame = this.animations[CurrentAnimationFrameIndex];
                nextFrame.Sprite.Visibility = Visibility.Visible;
            }

            CurrentAnimationFrameIndex++;
        }

        /// <summary>
        /// Collapses all death animation frames.
        /// </summary>
        public void CollapseAllAnimationFrames()
        {
            foreach (var frame in this.animations)
            {
                frame.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DeathAnimation> GetEnumerator()
        {
            return this.animations.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.animations.GetEnumerator();
        }
    }
}
