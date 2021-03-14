using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class CoordinateTest
    {
        [TestCase("A", 0)]
        [TestCase("J", 9)]
        public void ShouldMapColumnLetterCoordinatesToIndex(string column, int expected)
        {
            var c = new Coordinate
            {
                Column = column,
                Row = "1"
            };

            c.ColumnToIndex.Should().Be(expected);
        }

        [TestCase("1", 0)]
        [TestCase("10", 9)]
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
}