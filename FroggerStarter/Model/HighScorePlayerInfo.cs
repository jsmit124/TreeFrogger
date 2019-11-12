using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the player information to add to the high scores
    /// </summary>
    [Serializable]
    [XmlRootAttribute("HighScorePlayerInfo")]
    public class HighScorePlayerInfo
    {
        #region Methods

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Name + " | Score: " + Score + " | Level Completed: " + LevelCompleted;
        }

        #endregion

        /// <summary>Sorts players by score, then name, then level completed</summary>
        /// <seealso cref="HighScorePlayerInfo" />
        public class SortByScoreNameLevel : IComparer<HighScorePlayerInfo>
        {
            /// <summary>Compares the specified current.</summary>
            /// <param name="current">The current player.</param>
            /// <param name="other">The other player.</param>
            /// <returns></returns>
            public int Compare(HighScorePlayerInfo current, HighScorePlayerInfo other)
            {
                if (current != null && other != null)
                {
                    var scoreComparison = current.Score.CompareTo(other.Score) * -1;
                    if (scoreComparison != 0) return scoreComparison;

                    var nameComparison = string.Compare(current.Name, other.Name, StringComparison.Ordinal);
                    if (nameComparison != 0) return nameComparison;

                    return current.LevelCompleted.CompareTo(other.LevelCompleted);
                }

                return 0;
            }
        }

        /// <summary>Sorts the players by name, then score, then level completed</summary>
        /// <seealso cref="HighScorePlayerInfo" />
        public class SortByNameScoreLevel : IComparer<HighScorePlayerInfo>
        {
            /// <summary>Compares the specified current player.</summary>
            /// <param name="current">The current player.</param>
            /// <param name="other">The other player.</param>
            /// <returns></returns>
            public int Compare(HighScorePlayerInfo current, HighScorePlayerInfo other)
            {
                if (current != null && other != null)
                {
                    var nameComparison = string.Compare(current.Name, other.Name, StringComparison.Ordinal);
                    if (nameComparison != 0) return nameComparison;

                    var scoreComparison = current.Score.CompareTo(other.Score) * -1;
                    if (scoreComparison != 0) return scoreComparison;

                    return current.LevelCompleted.CompareTo(other.LevelCompleted);
                }

                return 0;
            }
        }

        /// <summary>sorts the players by level, then score, then name</summary>
        /// <seealso cref="HighScorePlayerInfo" />
        public class SortByLevelScoreName : IComparer<HighScorePlayerInfo>
        {
            /// <summary>Compares the specified current players.</summary>
            /// <param name="current">The current player.</param>
            /// <param name="other">The other player.</param>
            /// <returns></returns>
            public int Compare(HighScorePlayerInfo current, HighScorePlayerInfo other)
            {
                if (current != null && other != null)
                {
                    var levelComparison = current.LevelCompleted.CompareTo(other.LevelCompleted);
                    if (levelComparison != 0) return levelComparison;

                    var scoreComparison = current.Score.CompareTo(other.Score) * -1;
                    if (scoreComparison != 0) return scoreComparison;

                    return string.Compare(current.Name, other.Name, StringComparison.Ordinal);
                }

                return 0;
            }
        }

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
    }
}