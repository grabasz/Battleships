using System;

namespace Domain.Battleships.MapGeneraton
{
    public class RandomShipDataGenerator : IShipDataGenerator
    {
        private readonly Random _random = new Random();

        public int GetStartShipPoint(int shipSize)
        {
            return _random.Next(0, 10 - shipSize );
        }

        public int GetRand0To9()
        {
            return _random.Next(0, 10);
        }

        public bool GetIsVertical()
        {
            return _random.Next(0, 2).Equals(1);
        }

        public int GetDirection()
        {
            return _random.Next(0, 4);
        }
    }
}