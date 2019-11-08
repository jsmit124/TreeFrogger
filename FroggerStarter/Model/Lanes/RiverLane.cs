using System;
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
        public void HideAnotherVehicle()
        {
            for (var index = this.maxLogsInLane; index < this.NumberOfVehicles; index++)
            {
                if (this.NumberOfVehicles != 1)
                {
                    this.hideLog(index);
                }
            }
        }

        private void hideLog(int index)
        {
            if (this.nextVehicleIsReadyToBeVisible(index))
            {
                if (this.vehicleCrossedLeftBoundary(index))
                {
                    this.makeNextLogCollapsed(index);
                }
                else if (this.vehicleCrossedRightBoundary(index))
                {
                    this.makeNextLogCollapsed(index);
                }
            }
        }

        private void makeNextLogCollapsed(int index)
        {
            Vehicles[index].Sprite.Visibility = Visibility.Collapsed;
        }

        private void setupMaxLogsPerLane()
        {
            this.maxLogsInLane = Vehicles.Count;
        }

        private bool nextVehicleIsReadyToBeVisible(int i)
        {
            return Vehicles[i].Sprite.Visibility == Visibility.Visible;
        }

        private bool vehicleCrossedLeftBoundary(int i)
        {
            return Vehicles[i].Direction == Direction.Left &&
                   Math.Abs(Vehicles[i].X - (0.0 - Vehicles[i].Width)) <= 0;
        }

        private bool vehicleCrossedRightBoundary(int i)
        {
            return Vehicles[i].Direction == Direction.Right &&
                   Vehicles[i].X >= LaneSettings.LaneLength;
        }

        #endregion
    }
}