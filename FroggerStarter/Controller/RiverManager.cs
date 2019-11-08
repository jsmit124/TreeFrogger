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
    ///     Stores information for the RiverManager class
    /// </summary>
    public class RiverManager : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly IList<RiverLane> lanes;
        private readonly double topLaneYLocation;

        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="RiverManager" /> class.</summary>
        public RiverManager(double topLaneYLocation)
        {
            this.lanes = new List<RiverLane>();
            this.topLaneYLocation = topLaneYLocation;
            this.createLanes();
            this.setLogLocations();
            this.setupTimer();
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.lanes.SelectMany(lane => lane).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.lanes.SelectMany(lane => lane).GetEnumerator();
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
        ///     Increments the maximum amount of vehicles per lane.
        ///     Precondition: None
        ///     Postcondition: Max amount of vehicles per lane decreased by one
        /// </summary>
        public void DecrementMaxAmountOfVehiclesPerLane()
        {
            foreach (var lane in this.lanes)
            {
                lane.UpdateMaxVehiclesPerLane();
            }
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

        private void createLanes()
        {
            for (var i = 0; i < LaneSettings.RiverNumberOfVehicles.Length; i++)
            {
                var lane = new RiverLane(LaneSettings.RiverDirections[i], LaneSettings.RiverVehicleTypes[i],
                    LaneSettings.RiverNumberOfVehicles[i], LaneSettings.RiverSpeeds[i],
                    this.calculateNextLaneYLocation());
                this.lanes.Add(lane);
            }
        }

        private double calculateNextLaneYLocation()
        {
            return this.topLaneYLocation + (this.lanes.Count + 1) * LaneSettings.LaneWidth;
        }

        private static void setVehicleYLocations(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                vehicle.Y = (LaneSettings.LaneWidth - vehicle.Height) / 2 + lane.YLocation;
            }
        }

        private void setLogLocations()
        {
            foreach (var lane in this.lanes)
            {
                setVehicleYLocations(lane);
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
                lane.HideAnotherVehicle();
            }
        }

        #endregion
    }
}