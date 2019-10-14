using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    public class LaneManager
    {
        public IList<Lane> Lanes { get; private set; }
        public IEnumerable<Vehicle> AllVehicles => this.Lanes.SelectMany(lane => lane.Vehicles);

        public LaneManager()
        {
            this.Lanes = new List<Lane>();
            this.createLanes();
            this.setVehicleLocations();
        }

        private void createLanes()
        {
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.SportsCar, 3, 3.6, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.Semi, 2, 3.2, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.SportsCar, 3, 2.8, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Right, VehicleType.Semi, 3, 2.4, this.calculateNextLaneYLocation()));
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.SportsCar, 2, 2, this.calculateNextLaneYLocation()));
        }

        private int calculateNextLaneYLocation()
        {
            return Defaults.TopLaneYLocation + ((this.Lanes.Count + 1) * Defaults.LaneWidth);
        }

        public void IncrementSpeed(double increment)
        {
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
                vehicle.Y = ((Defaults.LaneWidth - vehicle.Height) / 2) + lane.YLocation;
            }
        }

        private static void setVehicleXLocations(Lane lane)
        {
            var distance = Defaults.LaneLength / lane.NumberOfVehicles;

            var count = 0;
            foreach (var vehicle in lane.Vehicles)
            {
                vehicle.X = count;
                count += (int) distance;
            }
        }

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

        public void ResetVehicleSpeedsToDefault()
        {
            foreach (var vehicle in this.AllVehicles)
            {
                vehicle.ResetSpeedToDefault();
            }
        }

    }
}
