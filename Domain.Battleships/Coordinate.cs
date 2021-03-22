using System;
using System.Text.RegularExpressions;

namespace Domain.Battleships
{
    public class Coordinate
    {
        public Coordinate(string row, string column)
        {
            Row = row;
            Column = column;
        }

        public static Coordinate FromSingleString(string rowColumn)
        {
            var match = Regex.Match(rowColumn, @"(\w{1})(\d+)");

            if (!match.Success)
                return null;

            var row = match.Groups[1].Value;
            var column = match.Groups[2].Value;

            return new Coordinate(row, column);
        }

        public static Coordinate FromIndex(int row, int column)
        {
            var columnString = (column + 1).ToString();
            var rowToLetter = Convert.ToChar(row + 65).ToString();

            return new Coordinate(rowToLetter, columnString);
        }

        public string Row { get; set; }
        public string Column { get; set; }

        public int RowToIndex => Row.ToUpper()[0] % 32 - 1;
        public int ColumnToIndex => int.Parse(Column) - 1;
        public override bool Equals(object obj)
        {
            if ((obj is Coordinate coordinate))
            {
                return coordinate.Row == Row && coordinate.Column == Column;
            }

            return false;
        }
    }
}