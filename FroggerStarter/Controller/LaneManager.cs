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

        private DispatcherTimer addCarsTimer;

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
            this.setupAddCarsTimer();
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
            this.lanes.Add(new Lane(LaneDirection.Right, VehicleType.PoliceCar, 3, 3.6,
                this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.Bus, 2, 3.2, this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 4, 2.8,
                this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Right, VehicleType.Bus, 3, 2.4, this.calculateNextLaneYLocation()));
            this.lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 5, 2,
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

        /// <summary>
        ///     Stops all vehicle movement.
        ///     Precondition: None
        ///     Postcondition: All vehicles in this.AllVehicles speed set to zero
        /// </summary>
        public void StopAllVehicleMovement()
        {
            this.lanes.SelectMany(lane => lane).ToList().ForEach(vehicle => vehicle.StopMovement());
        }

        private void setupAddCarsTimer()
        {
            this.addCarsTimer = new DispatcherTimer();
            this.addCarsTimer.Tick += this.addCarsTimerOnTick;
            this.addCarsTimer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            this.addCarsTimer.Start();
        }

        private void addCarsTimerOnTick(object sender, object e)
        {
            this.startMovingAnotherCarInEachLane();
        }

        private void startMovingAnotherCarInEachLane()
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveNextAvailableVehicle();
            }
        }

        /// <summary>
        ///     Resets the lanes to one vehicle.
        ///     Precondition: None
        ///     Postcondition: All lanes are reset to one moving vehicle
        /// </summary>
        public void ResetLanesToOneVehicle()
        {
            this.lanes.ToList().ForEach(lane => lane.ResetToOneVehicle());
            this.resetAddCarsTimer();
        }

        private void resetAddCarsTimer()
        {
            this.addCarsTimer.Stop();
            this.addCarsTimer.Start();
        }

        #endregion
    }
}