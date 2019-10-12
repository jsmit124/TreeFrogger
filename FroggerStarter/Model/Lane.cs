using System;
using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Enums;

namespace FroggerStarter.Model
{
    public class Lane : IEnumerable<Vehicle>
    {
        private double speed;
        private readonly int numberOfVehicles;
        private LaneDirection laneDirection;

        public ICollection<Vehicle> Vehicles { get; private set; }

        public Lane(LaneDirection direction, VehicleType vehicleType, int numberOfVehicles, double speed)
        {
            if (numberOfVehicles <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (speed <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.Vehicles = new List<Vehicle>();
            this.numberOfVehicles = numberOfVehicles;
            this.laneDirection = direction;
            this.speed = speed;

            this.populateLane(vehicleType);
        }

        private void populateLane(VehicleType vehicleType)
        {
            for (var i = 0; i < this.numberOfVehicles; i++)
            {
                this.Vehicles.Add(new Vehicle(vehicleType));
            }
        }

        public void IncrementSpeed(double increment)
        {
            this.speed += increment;
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
