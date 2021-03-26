using Domain.Battleships.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class CoordinateTest
    {
        [TestCase("1", 0)]
        [TestCase("10", 9)]
        public void ShouldMapColumnLetterCoordinatesToIndex(string column, int expected)
        {
            var c = new Coordinate("A",column);

            c.ColumnToIndex.Should().Be(expected);
        }

        [TestCase("A", 0)]
        [TestCase("J", 9)]
        public void ShouldMapRowIntCoordinatesToIndex(string row, int expected)
        {
            var c = new Coordinate(row, "1");

            c.RowToIndex.Should().Be(expected);
        }

        [Test]
        public void ShouldCreateCoordinateFromSingleString()
        {
            var c = Coordinate.FromSingleString("A1");

            c.Row.Should().Be("A");
            c.Column.Should().Be("1");

        }
    }
}