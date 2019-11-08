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
    /// <seealso cref="FrogHome" />
    public class FrogHomeManager : IEnumerable<FrogHome>
    {
        #region Data members

        private readonly IList<FrogHome> frogHomes;
        private readonly double homeYLocations;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrogHomeManager" /> class.
        /// </summary>
        public FrogHomeManager(double topLaneLocation)
        {
            this.frogHomes = new List<FrogHome>();
            this.homeYLocations = topLaneLocation;
            this.createFrogHomes();
            this.makeFrogHomesCollapsed();
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
        public IEnumerator<FrogHome> GetEnumerator()
        {
            return this.frogHomes.GetEnumerator();
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
            return this.frogHomes.GetEnumerator();
        }

        /// <summary>Makes the home frogs collapsed.</summary>
        public void makeFrogHomesCollapsed()
        {
            this.frogHomes.ToList().ForEach(homeFrog => homeFrog.Sprite.Visibility = Visibility.Collapsed);
        }

        private void createFrogHomes()
        {
            var count = 0;
            while (count < GameSettings.FrogHomeCount)
            {
                var frogHome = new FrogHome();
                var stepsBetweenHomes = 3;

                this.frogHomes.Add(frogHome);
                frogHome.X = stepsBetweenHomes * (frogHome.Width * (this.frogHomes.Count() - 1));
                frogHome.Y = this.homeYLocations;
                this.adjustEndFrogHomes(frogHome);
                count++;
            }
        }

        private void adjustEndFrogHomes(FrogHome homeFrog)
        {
            switch (this.frogHomes.Count)
            {
                case 1:
                    homeFrog.X += LaneSettings.EdgeOfScreenCushion;
                    break;
                case 5:
                    homeFrog.X -= LaneSettings.EdgeOfScreenCushion;
                    break;
            }
        }

        #endregion
    }
}