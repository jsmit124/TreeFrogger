using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FroggerStarter.Constants;
using FroggerStarter.Enums;

namespace FroggerStarter.Model.Lanes
{
    /// <summary>Stores information for the river lane class.</summary>
    /// <seealso cref="FroggerStarter.Model.Lanes.Lane" />
    public class RiverLane : Lane
    {
        #region Data members

        private int maxLogsInLane;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="RiverLane" /> class.</summary>
        /// <param name="direction">The direction.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <param name="numberOfVehicles">The number of vehicles.</param>
        /// <param name="defaultSpeed">The default speed.</param>
        /// <param name="yLocation">The y location.</param>
        public RiverLane(Direction direction, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed,
            double yLocation) : base(direction, vehicleType, numberOfVehicles, defaultSpeed, yLocation)
        {
            this.setupMaxLogsPerLane();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates the maximum vehicles per lane.
        /// </summary>
        public override void UpdateMaxVehiclesPerLane()
        {
            this.maxLogsInLane -= 1;
        }

        /// <summary>
        ///     Shows another vehicle.
        ///     Precondition: None.
        ///     Postcondition: Makes next index visible
        /// </summary>
        public async void HideAnotherVehicle()
        {
            for (var index = this.maxLogsInLane; index < this.NumberOfVehicles; index++)
            {
                await this.hideLog(index);
            }
        }

        /// <summary>
        ///     Hide all the vehicles except the first vehicle.
        ///     Precondition: None.
        ///     Postcondition: Makes all logs visible
        /// </summary>
        public void showAllLogs()
        {
            foreach (var vehicle in Vehicles)
            {
                vehicle.Sprite.Visibility = Visibility.Visible;
            }
        }

        private async Task hideLog(int index)
        {
            if (this.nextVehicleIsReadyToBeVisible(index))
            {
                if (this.vehicleCrossedLeftBoundary(index))
                {
                    this.makeNextLogCollapsed(index);
                    await Task.Delay(850);
                }
                else if (this.vehicleCrossedRightBoundary(index))
                {
                    this.makeNextLogCollapsed(index);
                    await Task.Delay(850);
                }
            }
        }

        private void makeNextLogCollapsed(int index)
        {
            Vehicles[index + 1].Sprite.Visibility = Visibility.Collapsed;
        }

        private void setupMaxLogsPerLane()
        {
            this.maxLogsInLane = Vehicles.Count;
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