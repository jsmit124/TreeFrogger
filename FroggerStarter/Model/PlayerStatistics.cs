﻿using FroggerStarter.Constants;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores basic information for the Player class
    /// </summary>
    public class PlayerStatistics
    {
        #region Properties

        /// <summary>
        ///     Gets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives { get; private set; }

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerStatistics" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Lives == Defaults.PlayerLives AND this.Score == 0
        /// </summary>
        public PlayerStatistics()
        {
            this.Lives = GameSettings.PlayerLives;
            this.Score = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Increments the score.
        ///     Precondition: None
        ///     Postcondition: this.Score == this.Score + 1
        /// </summary>
        public void IncrementScore()
        {
            this.Score += 1;
        }

        /// <summary>
        ///     Decrements the lives.
        ///     Precondition: None
        ///     Postcondition: this.Lives = this.Lives - 1
        /// </summary>
        public void DecrementLives()
        {
            this.Lives -= 1;
        }

        #endregion
    }
}