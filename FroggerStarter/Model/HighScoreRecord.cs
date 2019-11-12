using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the player information to add to the high scores
    /// </summary>
    [Serializable, XmlInclude(typeof(HighScorePlayerInfo))]
    public class HighScoreRecord : IEnumerable<HighScorePlayerInfo>
    {
        #region Data members

        /// <summary>The high scores</summary>
        [XmlArray("High Scores")]
        public readonly List<HighScorePlayerInfo> HighScores;

        #endregion

        #region Properties

        /// <summary>Gets or sets the <see cref="HighScorePlayerInfo" /> with the specified i.</summary>
        /// <param name="i">The i.</param>
        /// <value>The <see cref="HighScorePlayerInfo" />.</value>
        /// <returns></returns>
        public HighScorePlayerInfo this[int i]
        {
            get => this.HighScores[i];
            set => this.HighScores[i] = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScorePlayerInfo" /> class.
        /// </summary>
        public HighScoreRecord()
        {
            this.HighScores = new List<HighScorePlayerInfo>();
            this.HighScores.Add(new HighScorePlayerInfo("ABC", 4, 6));
            this.HighScores.Add(new HighScorePlayerInfo("DEF", 2, 2));
            this.HighScores.Add(new HighScorePlayerInfo("GHI", 3, 3));
            this.HighScores.Add(new HighScorePlayerInfo("JKL", 4, 4));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<HighScorePlayerInfo> IEnumerable<HighScorePlayerInfo>.GetEnumerator()
        {
            return this.HighScores.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.HighScores.GetEnumerator();
        }

        /// <summary>
        ///     Adds the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void AddInfo(HighScorePlayerInfo info)
        {
            this.HighScores.Add(info);
        }

        #endregion
    }
}