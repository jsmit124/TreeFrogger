using Windows.UI.Xaml;
using FroggerStarter.Enums;

namespace FroggerStarter.Constants
{
    /// <summary>
    ///     Stores information for the lane settings
    /// </summary>
    public static class LaneSettings
    {
        #region Data members

        /// <summary>The top lane y location</summary>
        public static double TopLaneYLocation = (double) Application.Current.Resources["HighRoadYLocation"];

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

        /// <summary>
        ///     The edge of screen cushion
        /// </summary>
        public static int EdgeOfScreenCushion = 15;

        /// <summary>
        ///     The river speeds
        /// </summary>
        public static double[] RiverSpeeds = {2.1, 1.6, 1.8, 1.6, 1.2};

        /// <summary>
        ///     The road speeds
        /// </summary>
        public static double[] RoadSpeeds = {3.5, 2.5, 5, 1.5, 1};

        /// <summary>
        ///     The road directions
        /// </summary>
        public static Direction[] RoadDirections =
            {Direction.Right, Direction.Left, Direction.Left, Direction.Right, Direction.Left};

        /// <summary>
        ///     The river directions
        /// </summary>
        public static Direction[] RiverDirections =
            {Direction.Right, Direction.Left, Direction.Right, Direction.Left, Direction.Right};

        /// <summary>
        ///     The road vehicle types
        /// </summary>
        public static VehicleType[] RoadVehicleTypes =
            {VehicleType.PoliceCar, VehicleType.Bus, VehicleType.SpeedCar, VehicleType.Bus, VehicleType.PoliceCar};

        /// <summary>
        ///     The river vehicle types
        /// </summary>
        public static VehicleType[] RiverVehicleTypes = {
            VehicleType.ShortLog, VehicleType.LongLog, VehicleType.ShortLog, VehicleType.LongLog, VehicleType.ShortLog
        };

        /// <summary>
        ///     The road number of vehicles
        /// </summary>
        public static int[] RoadNumberOfVehicles = {5, 3, 3, 3, 4};

        /// <summary>
        ///     The river number of vehicles
        /// </summary>
        public static int[] RiverNumberOfVehicles = {3, 2, 3, 2, 3};

        #endregion
    }
}