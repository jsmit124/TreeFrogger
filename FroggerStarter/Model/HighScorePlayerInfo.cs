using System;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores information for the player information to add to the high scores
    /// </summary>
    public class HighScorePlayerInfo
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; private set; }
        /// <summary>
        /// Gets the level completed.
        /// </summary>
        /// <value>
        /// The level completed.
        /// </value>
        public int LevelCompleted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScorePlayerInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        /// <param name="levelCompleted">The level completed.</param>
        public HighScorePlayerInfo(string name, int score, int levelCompleted)
        {
            this.Name = name;
            this.Score = score;
            this.LevelCompleted = levelCompleted;
        }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Name + " | Score: " + this.Score + " | Level Completed: " + this.LevelCompleted;
        }
    }
}