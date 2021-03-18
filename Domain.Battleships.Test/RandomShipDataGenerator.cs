using System;

namespace Domain.Battleships.Test
{
    public class RandomShipDataGenerator : IShipDataGenerator
    {
        private readonly Random _random = new Random();

        public int GetStartShipPoint(int shipSize)
        {
            return _random.Next(0, shipSize - 1);
        }

        public int GetConstantRowColumn()
        {
            return _random.Next(0, 9);
        }

        public bool GetIsVertical()
        {
            return _random.Next(0, 1).Equals(1);
        }
    }
}