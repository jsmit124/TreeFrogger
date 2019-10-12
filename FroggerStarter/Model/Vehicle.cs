using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    public class Vehicle : GameObject
    {
        public int SpeedXDirection { get; private set; }
        public int SpeedYDirection { get; private set; }

        public Vehicle(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.SportsCar:
                {
                    Sprite = new CarSprite();
                    break;
                }
                case VehicleType.Semi:
                    Sprite = new SemiSprite();
                    break;
            }
        
        SetSpeed(this.SpeedXDirection, this.SpeedYDirection);
        }

        public override void MoveRight()
        {
            base.MoveRight();

            if (base.X < 0 - base.Width)
            {
                base.X = 650;
            }
        }

        public override void MoveLeft()
        {
            base.MoveLeft();

            if (base.X > 650)
            {
                base.X = 0;
            }
        }

    }
}
