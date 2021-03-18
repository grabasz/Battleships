using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameTest
    {
        [Test]
        public void ShouldReturnStatusHitForFieldWithShip()
        {
            var map = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A","1"),
                    ShipBack = new Coordinate("A","5")
                }
            };

            var g = new Game(map);
            var c = new Coordinate("A","1");
            g.Play(c, Player.One).Should().Be(Status.Hit);
        }


        [Test]
        public void ShouldReturnStatusMissForFieldWithoutShip()
        {
            var map1 = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A","5")                }
            };

            var g = new Game(map1);
            var c = new Coordinate("B", "1");
            g.Play(c, Player.One).Should().Be(Status.Miss);
        }
    }

    public enum Player
    {
        One
    }

    public class Game
    {
        private readonly List<Ship> _map;

        public Game(List<Ship> map)
        {
            _map = map;
        }


        public Status Play(Coordinate coordinate, Player one)
        {
            if (IsAShip(coordinate))
                return Status.Hit;

            return Status.Miss;
        }

        private bool IsAShip(Coordinate coordinate)
        {
            return _map.Any(x => x.NotDestroyedPart.Contains(coordinate));
        }
    }
}