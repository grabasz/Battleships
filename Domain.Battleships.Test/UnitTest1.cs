using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameTest
    {
        [Test]
        public void ShouldInitializeGameWithOne5FlagShip()
        {
            Game g = new Game();

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

            g.Initialize(ships);
        }

        [TestCase("A",0)]
        [TestCase("J",9)]
        public void ShouldMapColumnLetterCoordinatesToIndex(string column, int expected)
        {
            var c = new Coordinate
            {
                Column = column,
                Row = "1"
            };

            c.ColumnToIndex.Should().Be(expected);
        }

        [TestCase("1",0)]
        [TestCase("10",9)]
        public void ShouldMapRowIntCoordinatesToIndex(string row, int expected)
        {
            var c = new Coordinate
            {
                Column = "A",
                Row = row
            };

            c.RowToIndex.Should().Be(expected);
        }

    }

    public class ShipCoordinates { 
        public Coordinate ShipFront { get; set; }
        public Coordinate ShipBack { get; set; }
    }

    public class Coordinate
    {
        public string Row { get; set; }
        public string Column { get; set; }

        public int RowToIndex => Int32.Parse(Row) - 1;
        public int ColumnToIndex => Column[0] %32  - 1;

    }

    public class Game
    {
        public void Initialize(List<ShipCoordinates> ships)
        {
            throw new System.NotImplementedException();
        }
    }
}