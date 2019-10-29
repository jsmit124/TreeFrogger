using System.Collections;
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
    public class LaneManager : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly ICollection<Lane> Lanes;

        private readonly double topLaneYLocation;

        #endregion

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
            return this.Lanes.SelectMany(lane => lane).GetEnumerator();
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
            return this.Lanes.SelectMany(lane => lane).GetEnumerator();
        }

        private void createLanes()
        {
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.PoliceCar, 3, 3.6,
                this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.Bus, 2, 3.2, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 3, 2.8,
                this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.Bus, 3, 2.4, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.PoliceCar, 2, 2,
                this.calculateNextLaneYLocation()));
        }

        private double calculateNextLaneYLocation()
        {
            return this.topLaneYLocation + (this.Lanes.Count + 1) * LaneSettings.LaneWidth;
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
        ///     Stops all vehicle movement.
        ///     Precondition: None
        ///     Postcondition: All vehicles in this.AllVehicles speed set to zero
        /// </summary>
        public void StopAllVehicleMovement()
        {
            this.Lanes.SelectMany(lane => lane).ToList().ForEach(vehicle => vehicle.StopMovement());
        }

        #endregion
    }
}