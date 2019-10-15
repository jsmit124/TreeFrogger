using System;
using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Stores information for the LaneManager class
    /// </summary>
    public class LaneManager
    {
        #region Properties

        /// <summary>
        ///     Gets the lanes.
        /// </summary>
        /// <value>
        ///     The lanes.
        /// </value>
        public IList<Lane> Lanes { get; }

        /// <summary>
        ///     Gets all vehicles.
        /// </summary>
        /// <value>
        ///     All vehicles.
        /// </value>
        public IEnumerable<Vehicle> AllVehicles => this.Lanes.SelectMany(lane => lane.Vehicles);

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LaneManager" /> class.
        /// </summary>
        public LaneManager()
        {
            this.Lanes = new List<Lane>();
            this.createLanes();
            this.setVehicleLocations();
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

        private int calculateNextLaneYLocation()
        {
            return Defaults.TopLaneYLocation + (this.Lanes.Count + 1) * Defaults.LaneWidth;
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
                vehicle.Y = (Defaults.LaneWidth - vehicle.Height) / 2 + lane.YLocation;
            }
        }

        private static void setVehicleXLocations(Lane lane)
        {
            var distance = Defaults.LaneLength / lane.NumberOfVehicles;

            var count = 0;
            foreach (var vehicle in lane.Vehicles)
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
            foreach (var lane in this.Lanes)
            {
                if (lane.Direction == LaneDirection.Left)
                {
                    moveAllVehiclesInLaneLeft(lane);
                }
                else
                {
                    moveAllVehiclesInLaneRight(lane);
                }
            }
        }

        private static void moveAllVehiclesInLaneLeft(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                vehicle.MoveLeft();
            }
        }

        private static void moveAllVehiclesInLaneRight(Lane lane)
        {
            foreach (var vehicle in lane)
            {
                vehicle.MoveRight();
            }
        }

        /// <summary>
        ///     Resets the vehicle speeds to default.
        ///     Precondition: None
        ///     Postcondition: each vehicle.SpeedX in this.AllVehicles == default speed for their lane
        /// </summary>
        public void ResetVehicleSpeedsToDefault()
        {
            foreach (var vehicle in this.AllVehicles)
            {
                vehicle.ResetSpeedToDefault();
            }
        }

        /// <summary>
        ///     Stops all vehicle movement.
        ///     Precondition: None
        ///     Postcondition: All vehicles in this.AllVehicles speed set to zero
        /// </summary>
        public void StopAllVehicleMovement()
        {
            foreach (var vehicle in this.AllVehicles)
            {
                vehicle.StopMovement();
            }
        }

        #endregion
    }
}