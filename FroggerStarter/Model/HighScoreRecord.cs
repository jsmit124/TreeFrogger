using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the player information to add to the high scores
    /// </summary>
    [XmlInclude(typeof(HighScoreRecord))]
    public class HighScoreRecord : IEnumerable, ISerializable
    {
        #region Data members

        /// <summary>The high scores</summary>
        public readonly IList<HighScorePlayerInfo> HighScores;

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
            this.HighScores.Add(new HighScorePlayerInfo("JIS", 5, 6));
        }

        #endregion

        #region Methods

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

        /// <summary>
        /// Adds the specified object to add.
        /// </summary>
        /// <param name="objectToAdd">The object to add.</param>
        public void Add(object objectToAdd)
        {
            if (objectToAdd.GetType() != typeof(HighScorePlayerInfo))
            {
                return;
            }
            else
            {
                this.AddInfo((HighScorePlayerInfo) objectToAdd);
            }
        }

        #endregion

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var item in this.HighScores)
            {
                info.AddValue("Name", item.Name);
                info.AddValue("Score", item.Score);
                info.AddValue("Level Completed", item.LevelCompleted);
            }
        }
    }
}