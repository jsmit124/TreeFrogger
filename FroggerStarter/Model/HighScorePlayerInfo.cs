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
        public String Name { get; private set; }
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
    }
}