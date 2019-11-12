using System;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the player information to add to the high scores
    /// </summary>
    [Serializable]
    [XmlRootAttribute("HighScorePlayerInfo")]
    public class HighScorePlayerInfo : IComparable<HighScorePlayerInfo>
    {
        #region Methods

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name + " | Score: " + Score + " | Level Completed: " + LevelCompleted;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [XmlAttribute("Name")]
        public string Name { get; }

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        [XmlAttribute("Score")]
        public int Score { get; }

        /// <summary>
        ///     Gets the level completed.
        /// </summary>
        /// <value>
        ///     The level completed.
        /// </value>
        [XmlAttribute("Level Completed")]
        public int LevelCompleted { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScorePlayerInfo" /> class.
        /// </summary>
        public HighScorePlayerInfo()
        {
            Name = "";
            Score = 0;
            LevelCompleted = 1;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScorePlayerInfo" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="score">The score.</param>
        /// <param name="levelCompleted">The level completed.</param>
        public HighScorePlayerInfo(string name, int score, int levelCompleted)
        {
            Name = name;
            Score = score;
            LevelCompleted = levelCompleted;
        }

        #endregion

        /// <summary>Compares to.</summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(HighScorePlayerInfo other)
        {
            var scoreComparison = Score.CompareTo(other.Score) * -1;
            if (scoreComparison != 0) return scoreComparison;

            var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
            if (nameComparison != 0) return nameComparison;

            return LevelCompleted.CompareTo(other.LevelCompleted);
        }
    }
}