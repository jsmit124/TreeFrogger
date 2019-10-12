using System;
using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Enums;

namespace FroggerStarter.Model
{
    public class Lane : IEnumerable<Vehicle>
    {
        public int NumberOfVehicles { get; private set; }
        public int YLocation { get; private set; }
        public LaneDirection Direction { get; private set; }

        public ICollection<Vehicle> Vehicles { get; private set; }

        public Lane(LaneDirection direction, VehicleType vehicleType, int numberOfVehicles, double defaultSpeed, int yLocation)
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

        private void populateLane(VehicleType vehicleType, double defaultSpeed)
        {
            for (var i = 0; i < this.NumberOfVehicles; i++)
            {
                this.Vehicles.Add(new Vehicle(vehicleType, defaultSpeed));
            }
        }

        public void IncrementSpeed(double increment)
        {
            foreach (var vehicle in this.Vehicles)
            {
                vehicle.IncrementSpeed(increment);
            }
        }

        public IEnumerator<Vehicle> GetEnumerator()
        {
            return this.Vehicles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Vehicles.GetEnumerator();
        }

    }
}
