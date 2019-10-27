using FroggerStarter.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information for the Lane class
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.Vehicle}" />
    public class Lane : IEnumerable<Vehicle>
    {
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

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public LaneDirection Direction { get; }

        /// <summary>
        ///     Gets the vehicles.
        /// </summary>
        /// <value>
        ///     The vehicles.
        /// </value>
        public ICollection<Vehicle> Vehicles { get; }

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

            this.Vehicles = new List<Vehicle>();
            this.NumberOfVehicles = numberOfVehicles;
            this.Direction = direction;
            this.YLocation = yLocation;

            this.populateLane(vehicleType, defaultSpeed);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
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
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Vehicles.GetEnumerator();
        }

        private void populateLane(VehicleType vehicleType, double defaultSpeed)
        {
            for (var i = 0; i < this.NumberOfVehicles; i++)
            {
                var vehicleToAdd = new Vehicle(vehicleType, defaultSpeed);

                if (this.Direction == LaneDirection.Right)
                {
                    vehicleToAdd.FlipSpriteHorizontal();
                }

                this.Vehicles.Add(vehicleToAdd);
            }
        }

        /// <summary>
        ///     Increments the speed.
        /// </summary>
        /// <param name="increment">The increment.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void IncrementSpeed(double increment)
        {
            if (increment <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var vehicle in this.Vehicles)
            {
                vehicle.IncrementSpeed(increment);
            }
        }

        #endregion
    }
}