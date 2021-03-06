﻿namespace FroggerStarter.Constants
{
    /// <summary>
    ///     Stores information for the game settings
    /// </summary>
    internal class GameSettings
    {
        #region Data members

        /// <summary>
        ///     The player lives
        /// </summary>
        public static int PlayerLives = 4;

        /// <summary>
        ///     The frog home count
        /// </summary>
        public static int FrogHomeCount = 5;

        /// <summary>
        ///     The time remaining at start of the game
        /// </summary>
        public static int TimeRemainingAtStart = 20;

        /// <summary>
        ///     The animation count
        /// </summary>
        public static int DeathAnimationCount = 4;

        /// <summary>
        ///     The player movement speed
        /// </summary>
        public const int PlayerMovementSpeed = 50;

        /// <summary>
        ///     The timer power ups
        /// </summary>
        public const int TimerPowerUps = 3;

        /// <summary>
        ///     The immunity power ups
        /// </summary>
        public const int ImmunityPowerUps = 2;

        /// <summary>
        ///     The final level
        /// </summary>
        public const int FinalLevel = 3;

        #endregion
    }
}