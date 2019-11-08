using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Factory;
using FroggerStarter.Model.Vehicles;

namespace FroggerStarter.Model.Lanes
{
    /// <summary>
    ///     Stores basic information for the Lane class
    /// </summary>
    /// <seealso cref="Vehicle" />
    public abstract class Lane : IEnumerable<Vehicle>
    {
        #region Data members

        /// <summary>The lane direction</summary>
        protected readonly Direction Direction;

        /// <summary>The vehicles</summary>
        protected readonly IList<Vehicle> Vehicles;

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
        protected Lane(Direction direction, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed,
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

            this.Vehicles = new List<Vehicle>();
            this.NumberOfVehicles = numberOfVehicles;
            this.Direction = direction;
            this.YLocation = yLocation;

            this.populateLane(vehicleType, defaultSpeed, direction);
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
            return this.Vehicles.GetEnumerator();
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
            return this.Vehicles.GetEnumerator();
        }

        /// <summary>Populates the lane.</summary>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="direction">The direction.</param>
        protected void populateLane(VehicleType vehicleType, double defaultSpeed, Direction direction)
        {
            var nextVehicleX = 0;
            for (var i = 0; i < this.NumberOfVehicles; i++)
            {
                var vehicleToAdd = VehicleFactory.BuildVehicle(vehicleType, direction, defaultSpeed);
                vehicleToAdd.X = nextVehicleX;
                nextVehicleX += (int) LaneSettings.LaneLength / this.NumberOfVehicles;
                if (this.Direction == Direction.Right)
                {
                    vehicleToAdd.FlipSpriteHorizontal();
                }

                this.Vehicles.Add(vehicleToAdd);
            }
        }

        /// <summary>
        ///     Moves the vehicles forward.
        ///     Precondition: None
        ///     Postcondition: all vehicles in the lane are moved forward
        /// </summary>
        public void MoveVehiclesForward()
        {
            var movingVehicles = (from vehicle in this.Vehicles select vehicle).ToList();
            foreach (var vehicle in movingVehicles)
            {
                vehicle.MoveForward();
            }
        }

        /// <summary>
        ///     Updates the maximum vehicles per lane.
        /// </summary>
        public abstract void UpdateMaxVehiclesPerLane();

        /// <summary>
        ///     Increases the vehicle speeds.
        ///     Precondition: None
        ///     Postcondition: All vehicle speeds increased by <see param="speed" />
        /// </summary>
        /// <param name="speed">The speed.</param>
        public void IncreaseVehicleSpeeds(double speed)
        {
            foreach (var vehicle in this.Vehicles)
            {
                vehicle.IncreaseSpeed(speed);
            }
        }

        #endregion
    }
}