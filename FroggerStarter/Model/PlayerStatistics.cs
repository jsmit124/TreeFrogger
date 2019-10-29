using FroggerStarter.Constants;

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

        /// <summary>
        ///     Gets the time remaining.
        /// </summary>
        /// <value>
        ///     The time remaining.
        /// </value>
        public int TimeRemaining { get; private set; }

        /// <summary>
        ///     Gets the amount of frogs in home.
        /// </summary>
        /// <value>
        ///     The amount of frogs in home.
        /// </value>
        public int AmountOfFrogsInHome { get; private set; }

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
            this.TimeRemaining = GameSettings.TimeRemainingAtStart;
            this.AmountOfFrogsInHome = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Increments the score.
        ///     Precondition: None
        ///     Postcondition: this.Score == this.Score + 1
        /// </summary>
        public void IncrementScore(int amountToIncrease)
        {
            this.Score += amountToIncrease;
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

        /// <summary>
        ///     Decrements the time remaining.
        ///     Precondition: None
        ///     Postcondition: this.TimeRemaining -= 1
        /// </summary>
        public void DecrementTimeRemaining()
        {
            this.TimeRemaining -= 1;
        }

        /// <summary>
        ///     Resets the time remaining.
        ///     Precondition: None
        ///     Postcondition: this.TimeRemaining = GameSettings.TimeRemainingAtStart
        /// </summary>
        public void ResetTimeRemaining()
        {
            this.TimeRemaining = GameSettings.TimeRemainingAtStart;
        }

        /// <summary>
        ///     Increments the frogs in homes.
        ///     Precondition: None
        ///     Postcondition: this.AmountOfFrogsInHome += 1
        /// </summary>
        public void IncrementFrogsInHomes()
        {
            this.AmountOfFrogsInHome += 1;
        }

        #endregion
    }
}