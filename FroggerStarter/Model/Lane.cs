using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Constants;
using FroggerStarter.Enums;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores basic information for the Lane class
    /// </summary>
    /// <seealso cref="Vehicle" />
    public class Lane : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly LaneDirection direction;

        private readonly ICollection<Vehicle> vehicles;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the number of vehicles.
        /// </summary>
        /// <value>
        ///     The number of vehicles.
        /// </value>
        public int NumberOfVehicles { get; }

        /// <summary>
        ///     Gets the y location.
        /// </summary>
        /// <value>
        ///     The y location.
        /// </value>
        public double YLocation { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lane" /> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="numberOfVehicles">The number of vehicles.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="yLocation">The y location.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public Lane(LaneDirection direction, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed,
            double yLocation)
        {
            if (numberOfVehicles <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (defaultSpeed <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.vehicles = new List<Vehicle>();
            this.NumberOfVehicles = numberOfVehicles;
            this.direction = direction;
            this.YLocation = yLocation;

            this.populateLane(vehicleType, defaultSpeed);
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
        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.vehicles.GetEnumerator();
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
            return this.vehicles.GetEnumerator();
        }

        private void populateLane(VehicleType vehicleType, double defaultSpeed)
        {
            for (var i = 0; i < this.NumberOfVehicles; i++)
            {
                var vehicleToAdd = new Vehicle(vehicleType, defaultSpeed) {
                    X = LaneSettings.LaneLength
                };

                if (this.direction == LaneDirection.Right)
                {
                    vehicleToAdd.FlipSpriteHorizontal();
                    vehicleToAdd.X = 0 - vehicleToAdd.Width;
                }

                if (this.vehicles.Count > 0)
                {
                    vehicleToAdd.StopMovement();
                }

                this.vehicles.Add(vehicleToAdd);
            }
        }

        /// <summary>
        ///     Moves the vehicles forward.
        ///     Precondition: None
        ///     Postcondition: all vehicles in the lane are moved forward
        /// </summary>
        public void MoveVehiclesForward()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.MoveForward(this.direction);
            }
        }

        /// <summary>
        ///     Moves the next available vehicle.
        ///     Precondition: None
        ///     Postcondition: Next available vehicle beings moving
        /// </summary>
        public void MoveNextAvailableVehicle()
        {
            var nextVehicle = this.vehicles.FirstOrDefault(vehicle => !vehicle.IsMoving);
            nextVehicle?.StartMovement();
        }

        /// <summary>
        ///     Resets to one vehicle.
        ///     Precondition: None
        ///     Postcondition: Lane is reset to only one moving vehicle
        /// </summary>
        public void ResetToOneVehicle()
        {
            this.resetAllVehiclesToStart();
            this.vehicles.ToList()[0].StartMovement();
        }

        private void resetAllVehiclesToStart()
        {
            var xLocation = LaneSettings.LaneLength;
            if (this.direction == LaneDirection.Right)
            {
                xLocation = 0 - this.vehicles.ToList()[0].Width;
            }

            foreach (var vehicle in this.vehicles)
            {
                vehicle.Reset(xLocation);
            }
        }

        #endregion
    }
}