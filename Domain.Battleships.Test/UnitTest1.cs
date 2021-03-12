using System.Collections.Generic;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameTest
    {
        [Test]
        public void ShouldInitializeGame()
        {
            Game g = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>();

            g.Initialize(ships);
        }

    }

    public class ShipCoordinates { 
        Coordinate ShipFront { get; set; }
        Coordinate ShipBack { get; set; }
    }

    internal class Coordinate
    {
        int Raw { get; set; }
        int Column { get; set; }
    }

    public class Game
    {
        public void Initialize(List<ShipCoordinates> ships)
        {
            throw new System.NotImplementedException();
        }
    }
}