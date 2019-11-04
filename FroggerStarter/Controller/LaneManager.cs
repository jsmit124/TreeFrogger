using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the LaneManager class
    /// </summary>
    public class LaneManager : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly ICollection<Lane> lanes;
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
            this.lanes = new List<Lane>();
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
            this.lanes.Add(new Lane(LaneDirection.Right, VehicleType.PoliceCar, 5, 3.5,
                this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.Bus, 3, 2.5, this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 4, 2,
                this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Right, VehicleType.Bus, 2, 1.5, this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 3, 1,
                this.calculateNextLaneYLocation()));
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

        /// <summary>Hides the vehicles.</summary>
        public void HideVehicles()
        {
            foreach (var lane in this.lanes)
            {
                lane.OnlyShowFirstVehicle();
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