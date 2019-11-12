using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the player information to add to the high scores
    /// </summary>
    [XmlInclude(typeof(HighScorePlayerInfo))]
    public class HighScorePlayerInfo 
    {
        #region Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; }

        /// <summary>
        ///     Gets the level completed.
        /// </summary>
        /// <value>
        ///     The level completed.
        /// </value>
        public int LevelCompleted { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScorePlayerInfo"/> class.
        /// </summary>
        public HighScorePlayerInfo()
        {
            this.Name = "";
            this.Score = 0;
            this.LevelCompleted = 1;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScorePlayerInfo" /> class.
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

        #endregion

        #region Methods

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Name + " | Score: " + this.Score + " | Level Completed: " + this.LevelCompleted;
        }

        #endregion
    }
}