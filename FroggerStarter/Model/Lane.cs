using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Factory;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores basic information for the Lane class
    /// </summary>
    /// <seealso cref="Vehicle" />
    public class Lane : IEnumerable<Vehicle>
    {
        #region Data members

        private readonly LaneDirection laneDirection;
        private readonly IList<Vehicle> vehicles;

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
        /// <param name="laneDirection">The laneDirection.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="numberOfVehicles">The number of vehicles.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="yLocation">The y location.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public Lane(LaneDirection laneDirection, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed,
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
            this.laneDirection = laneDirection;
            this.YLocation = yLocation;

            this.populateLane(vehicleType, defaultSpeed, laneDirection);
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

        private void populateLane(VehicleType vehicleType, double defaultSpeed, LaneDirection direction)
        {
            var nextVehicleX = 0;
            for (var i = 0; i < this.NumberOfVehicles; i++)
            {
                var vehicleToAdd = VehicleFactory.BuildVehicleSprite(vehicleType, direction, defaultSpeed);
                vehicleToAdd.X = nextVehicleX;
                nextVehicleX += (int) LaneSettings.LaneLength / this.NumberOfVehicles;
                if (this.laneDirection == LaneDirection.Right)
                {
                    vehicleToAdd.FlipSpriteHorizontal();
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
            var movingVehicles = (from vehicle in this.vehicles select vehicle).ToList();
            foreach (var vehicle in movingVehicles)
            {
                vehicle.MoveForward();
            }
        }

        /// <summary>Hide all the vehicles except the first vehicle.</summary>
        public void OnlyShowFirstVehicle()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.Sprite.Visibility = Visibility.Collapsed;
            }

            this.vehicles[0].Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>Shows another vehicle.</summary>
        public async void ShowAnotherVehicle()
        {
            for (var index = 0; index < this.vehicles.Count; index++)
            {
                await this.showVehicle(index);
            }
        }

        private async Task showVehicle(int index)
        {
            if (this.nextVehicleIsReadyToBeVisible(index))
            {
                if (this.vehicleCrossedLeftBoundary(index))
                {
                    this.makeNextVehicleVisible(index);
                    await Task.Delay(1000);
                }
                else if (this.vehicleCrossedRightBoundary(index))
                {
                    this.makeNextVehicleVisible(index);
                    await Task.Delay(1000);
                }
            }
        }

        private void makeNextVehicleVisible(int index)
        {
            this.vehicles[index + 1].Sprite.Visibility = Visibility.Visible;
        }

        private bool nextVehicleIsReadyToBeVisible(int i)
        {
            return this.vehicles[i].Sprite.Visibility == Visibility.Visible && i + 1 != this.vehicles.Count;
        }

        private bool vehicleCrossedLeftBoundary(int i)
        {
            return this.vehicles[i + 1].Direction == LaneDirection.Left &&
                   Math.Abs(this.vehicles[i + 1].X - (0.0 - this.vehicles[i + 1].Width)) <= 0;
        }

        private bool vehicleCrossedRightBoundary(int i)
        {
            return this.vehicles[i + 1].Direction == LaneDirection.Right &&
                   this.vehicles[i + 1].X >= LaneSettings.LaneLength;
        }

        #endregion
    }
}