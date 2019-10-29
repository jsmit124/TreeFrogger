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
        private DispatcherTimer deathAnimationTimer;
        private IList<DeathAnimation> animations;
        private int currentAnimationFrameIndex;

        /// <summary>
        /// The animation is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeathAnimationManager"/> class.
        /// </summary>
        public DeathAnimationManager()
        {
            this.animations = new List<DeathAnimation>();
            this.buildAnimationCollection();
            this.setupDeathAnimationTimer();
            this.resetFrameCount();
            this.collapseAllAnimationFrames();
        }

        private void resetFrameCount()
        {
            this.currentAnimationFrameIndex = 0;
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

        private void showNextFrame()
        {
            if (this.currentAnimationFrameIndex > 0)
            {
                var previousFrame = this.animations[currentAnimationFrameIndex];
                previousFrame.Sprite.Visibility = Visibility.Collapsed;
            }

            if (this.currentAnimationFrameIndex <= GameSettings.AnimationCount - 1)
            {
                var nextFrame = this.animations[currentAnimationFrameIndex];
                nextFrame.Sprite.Visibility = Visibility.Visible;
            }

            currentAnimationFrameIndex++;
        }
        
        private void setupDeathAnimationTimer()
        {
            this.deathAnimationTimer = new DispatcherTimer();
            this.deathAnimationTimer.Tick += this.deathAnimationTimerOnTick;
            this.deathAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void deathAnimationTimerOnTick(object sender, object e)
        {
            if (this.currentAnimationFrameIndex > GameSettings.AnimationCount - 1)
            {
                this.collapseAllAnimationFrames();
                this.deathAnimationTimer.Stop();
                this.resetFrameCount();
                this.IsRunning = false;
            }
            else
            {
                this.IsRunning = true;
                this.showNextFrame();
            }
        }

        private void collapseAllAnimationFrames()
        {
            foreach (var frame in this.animations)
            {
                frame.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Plays the animation.
        /// </summary>
        public void PlayDeathAnimation()
        {
            this.deathAnimationTimer.Start();
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
