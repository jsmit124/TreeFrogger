using System;
using System.Collections;
using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores information for the player information to add to the high scores
    /// </summary>
    public class HighScoreCollection :IEnumerable
    {
        private readonly IList<HighScorePlayerInfo> highScores;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighScorePlayerInfo"/> class.
        /// </summary>
        public HighScoreCollection()
        {
            this.highScores = new List<HighScorePlayerInfo>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.highScores.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified information.
        /// </summary>
        /// <param name="info">The information.</param>
        public void Add(HighScorePlayerInfo info)
        {
            this.highScores.Add(info);
        }
    }
}