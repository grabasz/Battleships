using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameTest
    {
        [Test]
        public void ShouldInitializeGameWithOne5FlagShipVertical()
        {
            Game game = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>
            {
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "A",
                        Row = "5"
                    }
                }
            };

            game.Initialize(ships);

            game.IsShip(0, 1).Should().BeFalse();
            game.IsShip(4, 0).Should().BeTrue();
        }
        
        [Test]
        public void ShouldInitializeGameWithOne5FlagShipHorizontal()
        {
            Game game = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>
            {
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "1"
                    }
                }
            };

            game.Initialize(ships);

            game.IsShip(0, 1).Should().BeTrue();
            game.IsShip(4, 0).Should().BeFalse();
        }

        [Test]
        public void ShouldThrowExceptionForDiagonalShip()
        {
            Game game = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>
            {
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "3"
                    }
                }
            };

            Assert.Throws<Exception>(() => game.Initialize(ships));
        }


        [Test]
        public void ShouldThrowExceptionForOverlapingShips()
        {
            Game game = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>
            {
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "1"
                    }
                },
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "1"
                    }
                }
            };

            Assert.Throws<Exception>(() => game.Initialize(ships));
        }

        [Test]
        public void ShouldAllowForNonOverlapingShips()
        {
            Game game = new Game();

            List<ShipCoordinates> ships = new List<ShipCoordinates>
            {
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "1"
                    }
                },
                new ShipCoordinates
                {
                    ShipFront = new Coordinate
                    {
                        Column = "A",
                        Row = "1"
                    },
                    ShipBack = new Coordinate
                    {
                        Column = "E",
                        Row = "1"
                    }
                }
            };

            Assert.Throws<Exception>(() => game.Initialize(ships));
        }



    }

    internal enum Status
    {
        blank,
        hit,
        miss,
        shipHasSunk
    }
}