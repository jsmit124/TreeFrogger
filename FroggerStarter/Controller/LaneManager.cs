using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Model.Lanes;
using FroggerStarter.Model.Vehicles;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the LaneManager class
    /// </summary>
    public class LaneManager : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly IList<RoadLane> lanes;
        private readonly double topLaneYLocation;

        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LaneManager" /> class.
        /// </summary>
        /// <param name="topLaneYLocation">The top lane y location.</param>
        public LaneManager(double topLaneYLocation)
        {
            this.lanes = new List<RoadLane>();
            this.topLaneYLocation = topLaneYLocation;
            this.createLanes();
            this.setVehicleLocations();
            this.setupTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.lanes.SelectMany(lane => lane).GetEnumerator();
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
            return this.lanes.SelectMany(lane => lane).GetEnumerator();
        }

        private void createLanes()
        {
            for (var i = 0; i < LaneSettings.RoadNumberOfVehicles.Length; i++)
            {
                var lane = new RoadLane(LaneSettings.RoadDirections[i], LaneSettings.RoadVehicleTypes[i],
                    LaneSettings.RoadNumberOfVehicles[i], LaneSettings.RoadSpeeds[i],
                    this.calculateNextLaneYLocation());
                this.lanes.Add(lane);
            }
        }

        private double calculateNextLaneYLocation()
        {
            return this.topLaneYLocation + (this.lanes.Count + 1) * LaneSettings.LaneWidth;
        }

        private void setVehicleLocations()
        {
            foreach (var lane in this.lanes)
            {
                setVehicleYLocations(lane);
            }
        }

        private static void setVehicleYLocations(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                vehicle.Y = (LaneSettings.LaneWidth - vehicle.Height) / 2 + lane.YLocation;
            }
        }

        /// <summary>
        ///     Moves the vehicles.
        ///     Precondition: None
        ///     Postcondition: each vehicle X is increased by their respective speed
        /// </summary>
        public void MoveVehicles()
        {
            this.lanes.ToList().ForEach(lane => lane.MoveVehiclesForward());
        }

        /// <summary>
        ///     Increases all vehicle speed.
        ///     Precondition: None
        ///     Postcondition: Increases all vehicle speeds by speed param
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void IncreaseAllVehicleSpeed(double speed)
        {
            foreach (var lane in this.lanes)
            {
                lane.IncreaseVehicleSpeeds(speed);
            }
        }

        /// <summary>
        ///     Hides the vehicles.
        ///     Precondition: None
        ///     Postcondition: Hides all vehicles except first one in each lane
        /// </summary>
        public void HideVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.OnlyShowFirstVehicle();
            }
        }

        /// <summary>
        ///     Increments the maximum amount of vehicles per lane.
        ///     Precondition: None
        ///     Postcondition: Max amount of vehicles per lane increased by one
        /// </summary>
        public void IncrementMaxAmountOfVehiclesPerLane()
        {
            foreach (var lane in this.lanes)
            {
                lane.UpdateMaxVehiclesPerLane();
            }
        }

        private void setupTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            foreach (var lane in this.lanes)
            {
                lane.ShowAnotherVehicle();
            }
        }

        #endregion
    }
}