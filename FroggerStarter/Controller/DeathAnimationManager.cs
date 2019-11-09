using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model.Animations;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the default animation manager class
    /// </summary>
    /// <seealso cref="DeathAnimation" />
    public class DeathAnimationManager : IEnumerable<DeathAnimation>
    {
        #region Data members

        /// <summary>
        ///     The animation over
        /// </summary>
        public EventHandler<EventArgs> AnimationOver;

        private readonly IList<DeathAnimation> animations;
        private DispatcherTimer deathAnimationTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the index of the current animation frame.
        /// </summary>
        /// <value>
        ///     The index of the current animation frame.
        /// </value>
        public int CurrentAnimationFrameIndex { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeathAnimationManager" /> class.
        /// </summary>
        public DeathAnimationManager()
        {
            this.animations = new List<DeathAnimation>();
            this.buildAnimationCollection();
            this.ResetFrameCount();
            this.CollapseAllAnimationFrames();
            this.setupDeathAnimationTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DeathAnimation> GetEnumerator()
        {
            return this.animations.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.animations.GetEnumerator();
        }

        /// <summary>
        ///     Resets the frame count.
        ///     Precondition: None
        ///     Postcondition: this.CurrentAnimationFrameIndex = 0
        /// </summary>
        public void ResetFrameCount()
        {
            this.CurrentAnimationFrameIndex = 0;
        }

        private void buildAnimationCollection()
        {
            for (var i = 1; i <= GameSettings.DeathAnimationCount; i++)
            {
                this.animations.Add(new DeathAnimation(i));
            }
        }

        /// <summary>
        ///     Sets the animation location.
        ///     Precondition: None
        ///     Postcondition: every animation in this.animations x, y = x, y
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
        ///     Shows the next death animation frame.
        ///     Precondition: None
        ///     Postcondition: currentAnimationFrameIndex++, prev frame = collapsed, new frame = visible
        /// </summary>
        public void ShowNextFrame()
        {
            this.animations[this.CurrentAnimationFrameIndex].Sprite.Visibility = Visibility.Collapsed;
            if (this.CurrentAnimationFrameIndex + 1 < GameSettings.DeathAnimationCount)
            {
                this.animations[this.CurrentAnimationFrameIndex + 1].Sprite.Visibility = Visibility.Visible;
            }

            this.CurrentAnimationFrameIndex++;
        }

        /// <summary>
        ///     Collapses all death animation frames.
        ///     Precondition: None
        ///     Postcondition: frame.Sprite.Visibility = Collapsed
        /// </summary>
        public void CollapseAllAnimationFrames()
        {
            foreach (var frame in this.animations)
            {
                frame.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        private void setupDeathAnimationTimer()
        {
            this.deathAnimationTimer = new DispatcherTimer();
            this.deathAnimationTimer.Tick += this.deathAnimationTimerOnTick;
            this.deathAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void deathAnimationTimerOnTick(object sender, object e)
        {
            if (this.CurrentAnimationFrameIndex > GameSettings.DeathAnimationCount - 1)
            {
                this.AnimationOver?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.ShowNextFrame();
            }
        }

        /// <summary>
        ///     Plays the death animation.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public void PlayDeathAnimation()
        {
            this.deathAnimationTimer.Start();
        }

        /// <summary>
        ///     Stops the animation timer.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public void StopAnimationTimer()
        {
            this.deathAnimationTimer.Stop();
        }

        /// <summary>
        ///     Rotates the sprite.
        ///     @Precondition: None
        ///     @Postcondition: Sprite is rotated
        /// </summary>
        public void RotateAllSprites(Direction direction)
        {
            foreach (var animation in this.animations)
            {
                animation.RotateSprite(direction);
            }
        }

        #endregion
    }
}