using System;
using FroggerStarter.Constants;
using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    public class Vehicle : GameObject
    {
        public Vehicle(VehicleType vehicleType, double defaultSpeed)
        {
            switch (vehicleType)
            {
                case VehicleType.SportsCar:
                    Sprite = new CarSprite();
                    break;
                case VehicleType.Semi:
                    Sprite = new SemiSprite();
                    break;
            }

            SetSpeed(defaultSpeed, 0);
        }

        public override void MoveRight()
        {
            base.MoveRight();

            if (base.X < 0 - base.Width)
            {
                base.X = Defaults.LaneLength;
            }
        }

        public override void MoveLeft()
        {
            base.MoveLeft();

            if (base.X > Defaults.LaneLength)
            {
                base.X = 0;
            }
        }

        public void IncrementSpeed(double amountToIncrement)
        {
            base.SetSpeed(base.SpeedX + amountToIncrement, 0);
        }

    }
}
