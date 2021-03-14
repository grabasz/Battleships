using System.Collections.Generic;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameGeneratorTest
    {

        [Test]
        public void ShouldCreateTheGame()
        {
            GameGenerator r = new GameGenerator();
            bool[,] map = r.Generate();
//            r.Generate();
        }

    }

    public class GameGenerator
    {
        private List<int> ShipLengths = new List<int>
        {
            5,
            4,
            4
        };

        public bool[,] Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}