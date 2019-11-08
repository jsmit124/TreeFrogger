using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;

namespace FroggerStarter.Model.Lanes
{
    /// <summary>Stores information for the road lane class.</summary>
    /// <seealso cref="FroggerStarter.Model.Lanes.Lane" />
    public class RoadLane : Lane
    {
        #region Data members

        private int maxCarsInLane;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="RoadLane" /> class.</summary>
        /// <param name="direction">The direction.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="numberOfVehicles">The number of vehicles.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="yLocation">The y location.</param>
        public RoadLane(Direction direction, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed,
            double yLocation) : base(direction, vehicleType, numberOfVehicles, defaultSpeed, yLocation)
        {
            this.setMaxCarsInLane();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Hide all the vehicles except the first vehicle.
        ///     Precondition: None.
        ///     Postcondition: Collapses all sprites except first index
        /// </summary>
        public void OnlyShowFirstVehicle()
        {
            foreach (var vehicle in Vehicles)
            {
                vehicle.Sprite.Visibility = Visibility.Collapsed;
            }

            Vehicles[0].Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Updates the maximum vehicles per lane.
        /// </summary>
        public void UpdateMaxVehiclesPerLane()
        {
            this.maxCarsInLane += 1;
        }

        /// <summary>
        ///     Shows another vehicle.
        ///     Precondition: None.
        ///     Postcondition: Makes next index visible
        /// </summary>
        public async void ShowAnotherVehicle()
        {
            for (var index = 0; index < this.maxCarsInLane; index++)
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
                    await Task.Delay(850);
                }
                else if (this.vehicleCrossedRightBoundary(index))
                {
                    this.makeNextVehicleVisible(index);
                    await Task.Delay(850);
                }
            }
        }

        private void setMaxCarsInLane()
        {
            this.maxCarsInLane = Vehicles.Count - 3;
        }

        private void makeNextVehicleVisible(int index)
        {
            Vehicles[index + 1].Sprite.Visibility = Visibility.Visible;
        }

        private bool nextVehicleIsReadyToBeVisible(int i)
        {
            return Vehicles[i].Sprite.Visibility == Visibility.Visible && i + 1 != Vehicles.Count;
        }

        private bool vehicleCrossedLeftBoundary(int i)
        {
            return Vehicles[i + 1].Direction == Direction.Left &&
                   Math.Abs(Vehicles[i + 1].X - (0.0 - Vehicles[i + 1].Width)) <= 0;
        }

        private bool vehicleCrossedRightBoundary(int i)
        {
            return Vehicles[i + 1].Direction == Direction.Right &&
                   Vehicles[i + 1].X >= LaneSettings.LaneLength;
        }

        #endregion
    }
}