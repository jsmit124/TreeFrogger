using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Factory;
using FroggerStarter.Model.PowerUps;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information about the power up manager.
    /// </summary>
    public class PowerUpManager : IEnumerable<PowerUp>
    {
        #region Data members

        private const int MaxPositionY = 605;
        private const int MinPositionY = 405;
        private const int MinPositionX = 0;

        private readonly IList<PowerUp> powerUps;
        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="PowerUpManager" /> class.</summary>
        public PowerUpManager()
        {
            this.powerUps = new List<PowerUp>();
            this.createAllPowerUps();
            this.setupTimer();
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<PowerUp> GetEnumerator()
        {
            return this.powerUps.GetEnumerator();
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
        }

        private void timerOnTick(object sender, object e)
        {
            var random = new Random();
            var index = random.Next(0, this.powerUps.Count - 1);
            this.powerUps[index].Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Starts the power up timer.
        ///     Precondition: none
        ///     Postcondition: power up timer started
        /// </summary>
        public void startPowerUpTimer()
        {
            this.timer.Start();
        }

        /// <summary>
        ///     Stops the power up timer.
        ///     Precondition: none
        ///     Postcondition: power up timer stopped
        /// </summary>
        public void stopPowerUpTimer()
        {
            this.timer.Stop();
        }

        private void createAllPowerUps()
        {
            this.createPowerUp(PowerUpType.Immunity, GameSettings.ImmunityPowerUps);
            this.createPowerUp(PowerUpType.Timer, GameSettings.TimerPowerUps);
        }

        private void createPowerUp(PowerUpType typeOfPowerUp, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var powerUp = PowerUpFactory.BuildPowerUp(typeOfPowerUp);
                var maxX = (int) (LaneSettings.LaneLength - powerUp.Width);
                setPowerUpPosition(powerUp, maxX);
                this.checkCollisionWithPowerUp(powerUp);
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                this.powerUps.Add(powerUp);
            }
        }

        private static void setPowerUpPosition(PowerUp powerUp, int maxX)
        {
            var random = new Random();
            powerUp.X = random.Next(MinPositionX, maxX);
            powerUp.Y = random.Next(MinPositionY, MaxPositionY);
        }

        private void checkCollisionWithPowerUp(PowerUp powerUp)
        {
            foreach (var timerPowerUp in this.powerUps)
            {
                while (timerPowerUp.CollisionDetected(powerUp))
                {
                    var maxX = (int) (LaneSettings.LaneLength - timerPowerUp.Width);
                    setPowerUpPosition(powerUp, maxX);
                }
            }
        }

        #endregion
    }
}