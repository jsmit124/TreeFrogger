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

        private const int MaxTimerPositionY = 305;
        private const int MinTimerPositionY = 105;
        private const int MaxImmunityPositionY = 605;
        private const int MinImmunityPositionY = 405;
        private const int MinPositionX = 0;

        private int currentPowerUpIndex;

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
            if (this.currentPowerUpIndex < this.powerUps.Count)
            {
                this.powerUps[this.currentPowerUpIndex].Sprite.Visibility = Visibility.Visible;
                this.currentPowerUpIndex++;
            }
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
            if (powerUp.PowerUpType == PowerUpType.Timer)
            {
                powerUp.X = random.Next(MinPositionX, maxX);
                powerUp.Y = random.Next(MinTimerPositionY, MaxTimerPositionY);
            }
            else
            {
                powerUp.X = random.Next(MinPositionX, maxX);
                powerUp.Y = random.Next(MinImmunityPositionY, MaxImmunityPositionY);
            }
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