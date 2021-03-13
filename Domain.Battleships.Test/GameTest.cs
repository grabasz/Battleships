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
        public int ColumnToIndex => Column.ToUpper()[0] %32  - 1;

    }

    public class Game
    {
        private const int BoardSize = 10;
        private readonly bool[,] _board = new bool[BoardSize, BoardSize];
        public void Initialize(List<ShipCoordinates> ships)
        {
            foreach (var ship in ships)
            {
                for (var rowIndex = 0; rowIndex < BoardSize; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < BoardSize; columnIndex++)
                    {
                        if (IsShipWithinCoordinates(ship, rowIndex, columnIndex))
                            _board[rowIndex,columnIndex] = true;

                        Console.Write(_board[rowIndex, columnIndex] + " ");
                    }

                    Console.WriteLine();
                }
            }
        }

        private bool IsShipWithinCoordinates(ShipCoordinates ship, int rowIndex, int columnIndex)
        {
            if (IsShipHorizontal(ship))
            {
                if (IsNotMatchingRowColumn(ship.ShipFront.RowToIndex, rowIndex))
                    return false;

                return IsOccupiedByShip(rowIndex, ship.ShipFront.ColumnToIndex, ship.ShipBack.ColumnToIndex);
            }

            if (IsShipVertical(ship))
            {
                if (IsNotMatchingRowColumn(ship.ShipFront.ColumnToIndex, columnIndex))
                    return false;

                return IsOccupiedByShip(rowIndex, ship.ShipFront.RowToIndex, ship.ShipBack.RowToIndex);
            }

            throw new Exception("Unable to put ship diagonally");

        }

        private static bool IsOccupiedByShip(int rowIndex, int shipFrontRowToIndex, int shipBackRowToIndex)
        {
            return (shipFrontRowToIndex - shipBackRowToIndex) > 0
                ? IsWithinRange(rowIndex, shipBackRowToIndex, shipFrontRowToIndex)
                : IsWithinRange(rowIndex, shipFrontRowToIndex, shipBackRowToIndex);
        }

        private static bool IsWithinRange(int rowIndex, int shipLowerIndex, int shipGreaterIndex)
        {
            return shipLowerIndex <= rowIndex && shipGreaterIndex >= rowIndex;
        }

        private static bool IsNotMatchingRowColumn(int shipFrontColumnToIndex, int columnIndex)
        {
            return columnIndex != shipFrontColumnToIndex;
        }

        private static bool IsShipHorizontal(ShipCoordinates ship)
        {
            return ship.ShipFront.RowToIndex == ship.ShipBack.RowToIndex;
        }

        private static bool IsShipVertical(ShipCoordinates ship)
        {
            return ship.ShipFront.ColumnToIndex == ship.ShipBack.ColumnToIndex;
        }

        public bool IsShip(int row, int column)
        {
            return _board[row,column];
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