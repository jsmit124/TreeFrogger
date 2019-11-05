using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information about the power up manager.
    /// </summary>
    public class TimerPowerUpManager : IEnumerable<TimerPowerUp>
    {
        #region Data members

        private int currentPowerUpIndex;

        private readonly IList<TimerPowerUp> timerPowerUps;
        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimerPowerUpManager" /> class.</summary>
        public TimerPowerUpManager()
        {
            this.timerPowerUps = new List<TimerPowerUp>();
            this.createTimerPowerUps();
            this.setupTimer();
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TimerPowerUp> GetEnumerator()
        {
            return this.timerPowerUps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void setupTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 10);
            this.timer.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            if (this.currentPowerUpIndex < GameSettings.TimerPowerUps)
            {
                this.timerPowerUps[this.currentPowerUpIndex].Sprite.Visibility = Visibility.Visible;
                this.currentPowerUpIndex++;
            }
        }

        private void createTimerPowerUps()
        {
            var random = new Random();
            for (var i = 0; i < GameSettings.TimerPowerUps; i++)
            {
                var powerUp = new TimerPowerUp();
                var maxX = (int)(LaneSettings.LaneLength - powerUp.Width);
                powerUp.X = random.Next(0, maxX);
                powerUp.Y = random.Next(55, 305);
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                this.timerPowerUps.Add(powerUp);
            }
        }

        #endregion
    }
}