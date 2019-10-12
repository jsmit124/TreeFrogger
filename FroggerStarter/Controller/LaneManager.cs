using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Enums;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    public class LaneManager
    {
        public IList<Lane> Lanes { get; private set; }

        public LaneManager()
        {
            this.Lanes = new List<Lane>();
            this.Lanes.Add(new Lane(LaneDirection.Left, VehicleType.Semi, 2, 2));
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            return this.Lanes.SelectMany(lane => lane.Vehicles);
        }
        


    }
}
