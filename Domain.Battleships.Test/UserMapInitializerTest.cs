using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class UserMapInitializerTest
    {
        [Test]
        public void ShouldInitializeGameWithOne5FlagShipVertical()
        {
            UserMapInitializer userMapInitializer = new UserMapInitializer();

            List<Ship> ships = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A", "5")
                }
            };

            var map =userMapInitializer.Initialize(ships);

            map[0, 1].Should().BeTrue();
            map[1, 0].Should().BeFalse();
        }
        
        [Test]
        public void ShouldInitializeGameWithOne5FlagShipHorizontal()
        {
            UserMapInitializer userMapInitializer = new UserMapInitializer();

            List<Ship> ships = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("E", "1")
                }
            };

            var map = userMapInitializer.Initialize(ships);

            map[1, 0].Should().BeTrue();
            map[0, 1].Should().BeFalse();
        }

        [Test]
        public void ShouldThrowExceptionForDiagonalShip()
        {
            UserMapInitializer userMapInitializer = new UserMapInitializer();

            List<Ship> ships = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("E", "3")
                }
            };

            Assert.Throws<Exception>(() => userMapInitializer.Initialize(ships));
        }


        [Test]
        public void ShouldThrowExceptionForOverlapingShips()
        {
            UserMapInitializer userMapInitializer = new UserMapInitializer();

            List<Ship> ships = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A", "5")
                },
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A", "5")
                }
            };

            Assert.Throws<Exception>(() => userMapInitializer.Initialize(ships));
        }

        [Test]
        [Ignore("The class will be deleted")]
        public void ShouldAllowForNonOverlapingShips()
        {
            UserMapInitializer userMapInitializer = new UserMapInitializer();

            List<Ship> ships = new List<Ship>
            {
                new Ship
                {
                    ShipFront = new Coordinate("A", "1"),
                    ShipBack = new Coordinate("A", "5")
                },
                new Ship
                {
                    ShipFront = new Coordinate("B", "1"),
                    ShipBack = new Coordinate("B", "5")
                }
            };
            userMapInitializer.Initialize(ships);
        }



    }
}