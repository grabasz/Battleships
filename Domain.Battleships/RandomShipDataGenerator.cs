using System;

namespace Domain.Battleships
{
    public class RandomShipDataGenerator : IShipDataGenerator
    {
        private readonly Random _random = new Random();

        public int GetStartShipPoint(int shipSize)
        {
            return _random.Next(0, 10 - shipSize );
        }

        public int GetConstantRowColumn()
        {
            return _random.Next(0, 10);
        }

        public bool GetIsVertical()
        {
            return _random.Next(0, 1).Equals(1);
        }
    }
}