using FroggerStarter.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Stores information for the homefrog manager
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.HomeFrog}" />
    public class HomeFrogManager : IEnumerable<HomeFrog>
    {

        private IList<HomeFrog> homeFrogs;
        private double homeYLocations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeFrogManager"/> class.
        /// </summary>
        public HomeFrogManager(double topLaneLocation)
        {
            this.homeFrogs = new List<HomeFrog>();
            this.homeYLocations = topLaneLocation;
            this.createHomeFrogs();
            this.makeHomeFrogsCollapsed();
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
            this.homeFrogs.ToList().ForEach(homeFrog => homeFrog.Sprite.Visibility = Windows.UI.Xaml.Visibility.Collapsed);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<HomeFrog> GetEnumerator()
        {
            return this.homeFrogs.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.homeFrogs.GetEnumerator();
        }
    }
}
