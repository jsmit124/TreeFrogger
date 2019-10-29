using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the homefrog manager
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.HomeFrog}" />
    public class HomeFrogManager : IEnumerable<HomeFrog>
    {
        #region Data members

        private readonly IList<HomeFrog> homeFrogs;
        private readonly double homeYLocations;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeFrogManager" /> class.
        /// </summary>
        public HomeFrogManager(double topLaneLocation)
        {
            this.homeFrogs = new List<HomeFrog>();
            this.homeYLocations = topLaneLocation;
            this.createHomeFrogs();
            this.makeHomeFrogsCollapsed();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<HomeFrog> GetEnumerator()
        {
            return this.homeFrogs.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.homeFrogs.GetEnumerator();
        }

        private void createHomeFrogs()
        {
            var count = 0;
            while (count < GameSettings.FrogHomeCount)
            {
                var homeFrog = new HomeFrog();
                var stepsBetweenHomes = 3;

                this.homeFrogs.Add(homeFrog);
                homeFrog.X = stepsBetweenHomes * (homeFrog.Width * (this.homeFrogs.Count() - 1));
                homeFrog.Y = this.homeYLocations;

                count++;
            }
        }

        private void makeHomeFrogsCollapsed()
        {
            this.homeFrogs.ToList().ForEach(homeFrog => homeFrog.Sprite.Visibility = Visibility.Collapsed);
        }

        #endregion
    }
}