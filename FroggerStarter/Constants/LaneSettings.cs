using Windows.UI.Xaml;

namespace FroggerStarter.Constants
{
    /// <summary>
    ///     Stores information for the lane settings
    /// </summary>
    public static class LaneSettings
    {
        #region Data members

        /// <summary>The top lane y location</summary>
        public static double TopLaneYLocation = (double)Application.Current.Resources["HighRoadYLocation"];

        /// <summary>The middle safe lane location</summary>
        public static double MiddleSafeLaneLocation = 355;

        /// <summary>
        ///     The bottom lane offset
        /// </summary>
        public static int BottomLaneOffset = 5;

        /// <summary>
        ///     The lane width
        /// </summary>
        public static int LaneWidth = 50;

        /// <summary>
        ///     The lane length
        /// </summary>
        public static double LaneLength = 650;

        #endregion
    }
}