using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the LaneManager class
    /// </summary>
    public class LaneManager : IEnumerable<Vehicle>
    {
        #region Properties

        /// <summary>
        ///     Gets the lanes.
        /// </summary>
        /// <value>
        ///     The lanes.
        /// </value>
        public IList<Lane> Lanes { get; }

        #endregion

        private DispatcherTimer speedTimer;
        private double topLaneYLocation;

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LaneManager" /> class.
        /// </summary>
        public LaneManager(double topLaneYLocation)
        {
            this.Lanes = new List<Lane>();
            this.topLaneYLocation = topLaneYLocation;
            this.createLanes();
            this.setVehicleLocations();
            this.setupSpeedTimer();
        }

        #endregion

        #region Methods

        private void createLanes()
        {
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.SportsCar, 3, 3.6,
                this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.Semi, 2, 3.2, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.SportsCar, 3, 2.8,
                this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.Semi, 3, 2.4, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.SportsCar, 2, 2,
                this.calculateNextLaneYLocation()));
        }

        private double calculateNextLaneYLocation()
        {
            return this.topLaneYLocation + (this.Lanes.Count + 1) * LaneSettings.LaneWidth;
        }

        /// <summary>
        ///     Increments the speed.
        ///     Precondition: increment > 0
        ///     Postcondition: speed is increased for each vehicle in this.Lanes
        /// </summary>
        /// <param name="increment">The increment.</param>
        public void IncrementSpeed(double increment)
        {
            if (increment <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var lane in this.Lanes)
            {
                lane.IncrementSpeed(increment);
            }
        }

        private void setVehicleLocations()
        {
            foreach (var lane in this.Lanes)
            {
                setVehicleXLocations(lane);
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

        private static void setVehicleXLocations(Lane lane)
        {
            var distance = LaneSettings.LaneLength / lane.NumberOfVehicles;

            var count = 0;
            foreach (var vehicle in lane)
            {
                vehicle.X = count;
                count += distance;
            }
        }

        /// <summary>
        ///     Moves the vehicles.
        ///     Precondition: None
        ///     Postcondition: each vehicle X is increased by their respective speed
        /// </summary>
        public void MoveVehicles()
        {
            this.Lanes.ToList().ForEach(lane => lane.MoveVehiclesForward());
        }

        /// <summary>
        ///     Resets the vehicle speeds to default.
        ///     Precondition: None
        ///     Postcondition: each vehicle.SpeedX in this.AllVehicles == default speed for their lane
        /// </summary>
        public void ResetVehicleSpeedsToDefault()
        {
            this.Lanes.SelectMany(lane => lane).ToList().ForEach(vehicle => vehicle.ResetSpeedToDefault());
        }

        /// <summary>
        ///     Stops all vehicle movement.
        ///     Precondition: None
        ///     Postcondition: All vehicles in this.AllVehicles speed set to zero
        /// </summary>
        public void StopAllVehicleMovement()
        {
            this.Lanes.SelectMany(lane => lane).ToList().ForEach(vehicle => vehicle.StopMovement());
        }

        private void setupSpeedTimer()
        {
            this.speedTimer = new DispatcherTimer();
            this.speedTimer.Tick += this.speedTimerOnTick;
            this.speedTimer.Interval = new TimeSpan(0, 0, 1);
            this.speedTimer.Start();
        }

        private void speedTimerOnTick(object sender, object e)
        {
            this.IncrementSpeed(0.1);
        }

        /// <summary>
        /// Stops the speed timer.
        /// Precondition: None
        /// Postcondition: this.speedTimer is stopped
        /// </summary>
        public void StopSpeedTimer()
        {
            this.speedTimer.Stop();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.Lanes.SelectMany(lane => lane).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Lanes.SelectMany(lane => lane).GetEnumerator();
        }

        #endregion
    }
}