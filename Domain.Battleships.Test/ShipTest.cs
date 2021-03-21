using System;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class ShipTest
    {
        [Test]
        public void ShouldGetAllShipPointsForHorizontalShip()
        {
            var s = new Ship
            {
                ShipFront = new Coordinate("A", "1"),
                ShipBack = new Coordinate("A", "5")
            };

            s.GetAllPoints().Should().BeEquivalentTo(
                new Coordinate("A", "1"),
                new Coordinate("A", "2"),
                new Coordinate("A", "3"),
                new Coordinate("A", "4"),
                new Coordinate("A", "5"));
        }

        [Test]
        public void ShouldGetAllShipPointsForVerticalShip()
        {
            var s = new Ship
            {
                ShipFront = new Coordinate("A", "1"),
                ShipBack = new Coordinate("E", "1")
            };

            s.GetAllPoints().Should().BeEquivalentTo(
                new Coordinate("A", "1"),
                new Coordinate("B", "1"),
                new Coordinate("C", "1"),
                new Coordinate("D", "1"),
                new Coordinate("E", "1")
            );
        }

        [Test]
        public void ShouldThrowExceptionForDiagonalShip()
        {
            var s = new Ship
            {
                ShipFront = new Coordinate("A", "1"),
                ShipBack = new Coordinate("E", "5")
            };

            Assert.Throws<Exception>(() => s.GetAllPoints());
        }
    }
}