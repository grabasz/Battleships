using System.Collections.Generic;
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

            var g = new GameChecker(map);
            var c = new Coordinate("A","1");
            g.Play(c).Should().Be(Status.Hit);
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

            var g = new GameChecker(map1);
            var c = new Coordinate("B", "1");
            g.Play(c).Should().Be(Status.Miss);
        }

        [Test]
        public void ShouldReturnStatusShipHasSankWhenAllShipPartsWereHit()
        {
            var map1 = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A","3")                }
            };

            var g = new GameChecker(map1);
            g.Play(new Coordinate("A", "1"));
            g.Play(new Coordinate("A", "2"));
            g.Play(new Coordinate("A", "3")).Should().Be(Status.ShipHasSunk);
        }
    }

    public enum Player
    {
        One
    }
}