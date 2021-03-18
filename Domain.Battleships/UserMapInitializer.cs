using System;
using System.Collections.Generic;

namespace Domain.Battleships
{
    public interface IUserMapInitializer
    {
        bool[,] Initialize(List<Ship> ships);
    }

    public class UserMapInitializer : IUserMapInitializer
    {
        private const int BoardSize = 10;
        public bool[,] Initialize(List<Ship> ships)
        {
            bool[,] _board = new bool[BoardSize, BoardSize];
            foreach (var ship in ships)
            {
                for (var rowIndex = 0; rowIndex < BoardSize; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < BoardSize; columnIndex++)
                    {
                        if (_board[rowIndex, columnIndex])
                            throw new Exception("Field already occupied by other ship");
                        if (IsShipWithinCoordinates(ship, rowIndex, columnIndex))
                            _board[rowIndex,columnIndex] = true;

                        Console.Write(_board[rowIndex, columnIndex] + " ");
                    }

                    Console.WriteLine();
                }
            }

            return _board;
        }

        private bool IsShipWithinCoordinates(Ship ship, int rowIndex, int columnIndex)
        {
            if (IsShipHorizontal(ship))
            {
                if (IsNotMatchingRowColumn(ship.ShipFront.RowToIndex, rowIndex))
                    return false;

                return IsOccupiedByShip(columnIndex, ship.ShipFront.ColumnToIndex, ship.ShipBack.ColumnToIndex);
            }

            if (IsShipVertical(ship))
            {
                if (IsNotMatchingRowColumn(ship.ShipFront.ColumnToIndex, columnIndex))
                    return false;

                return IsOccupiedByShip(rowIndex, ship.ShipFront.RowToIndex, ship.ShipBack.RowToIndex);
            }

            throw new Exception("Unable to put ship diagonally");
        }

        private static bool IsOccupiedByShip(int index, int shipFrontIndex, int shipBackIndex)
        {
            return (shipFrontIndex - shipBackIndex) > 0
                ? IsWithinRange(index, shipBackIndex, shipFrontIndex)
                : IsWithinRange(index, shipFrontIndex, shipBackIndex);
        }

        private static bool IsWithinRange(int index, int lowerIndex, int greaterIndex)
        {
            return lowerIndex <= index && greaterIndex >= index;
        }

        private static bool IsNotMatchingRowColumn(int shipFrontColumnToIndex, int columnIndex)
        {
            return columnIndex != shipFrontColumnToIndex;
        }

        private static bool IsShipHorizontal(Ship ship)
        {
            return ship.ShipFront.RowToIndex == ship.ShipBack.RowToIndex;
        }

        private static bool IsShipVertical(Ship ship)
        {
            return ship.ShipFront.ColumnToIndex == ship.ShipBack.ColumnToIndex;
        }
    }
}