using FroggerStarter.Constants;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores basic information for the Player class
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the lives.
        /// </summary>
        /// <value>
        /// The lives.
        /// </value>
        public int Lives { get; private set; }
        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// Precondition: None
        /// Postcondition: this.Lives == Defaults.PlayerLives AND this.Score == 0
        /// </summary>
        public Player()
        {
            this.Lives = Defaults.PlayerLives;
            this.Score = 0;
        }

        /// <summary>
        /// Increments the score.
        /// Precondition: None
        /// Postcondition: this.Score == this.Score + 1
        /// </summary>
        public void IncrementScore()
        {
            this.Score += 1;
        }

        /// <summary>
        /// Decrements the lives.
        /// Precondition: None
        /// Postcondition: this.Lives = this.Lives - 1
        /// </summary>
        public void DecrementLives()
        {
            this.Lives -= 1;
        }
    }
}
