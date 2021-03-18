using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class MapGeneratorTest
    {
        [Test]
        public void ShouldCreate10x10Map()
        {
            MapGenerator mapGenerator = new MapGenerator(new RandomShipDataGenerator());
            bool[,] map = mapGenerator.Generate(new List<int>());
            map.GetLongLength(0).Should().Be(10);
            map.GetLongLength(1).Should().Be(10);
        }

        [Test]
        [Repeat(100)]
        public void ShouldInitializeMapShip100TimesToVerifyIfRandomCanCauseTheError()
        {
        List<int> shipLengths = new List<int>
        {
            5,
            4,
            4
        };
        MapGenerator mapGenerator = new MapGenerator(new RandomShipDataGenerator());
            bool[,] map = mapGenerator.Generate(shipLengths);
            map.Should().Contain(true);
        }

        [Test]
        public void ShouldInitializeMapWithMockGeneratorWillPlaceShipVertical()
        {
            List<int> shipLengths = new List<int>
            {
                5,
            };
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            MapGenerator r = new MapGenerator(shipDataGeneratorMock.Object);

            shipDataGeneratorMock.Setup(m => m.GetStartShipPoint(5)).Returns(0);
            shipDataGeneratorMock.Setup(m => m.GetConstantRowColumn()).Returns(0);
            shipDataGeneratorMock.Setup(m => m.GetIsVertical()).Returns(true);

            bool[,] map = r.Generate(shipLengths);
            map[0,0].Should().BeTrue();
            map[4,0].Should().BeTrue();
            map[5,0].Should().BeFalse();
        }

        [Test]
        public void ShouldInitializeMapWithMockGeneratorWillPlaceShipHorizontal()
        {
            List<int> shipLengths = new List<int>
            {
                5,
            };
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            MapGenerator r = new MapGenerator(shipDataGeneratorMock.Object);

            shipDataGeneratorMock.Setup(m => m.GetStartShipPoint(5)).Returns(0);
            shipDataGeneratorMock.Setup(m => m.GetConstantRowColumn()).Returns(0);
            shipDataGeneratorMock.Setup(m => m.GetIsVertical()).Returns(false);

            bool[,] map = r.Generate(shipLengths);
            map[0, 0].Should().BeTrue();
            map[0, 4].Should().BeTrue();
            map[0, 5].Should().BeFalse();
        }
    }

    public interface IShipDataGenerator
    {
        int GetStartShipPoint(int shipSize);
        int GetConstantRowColumn();
        bool GetIsVertical();
    }

    public interface IMapGenerator
    {
        bool[,] Generate(List<int> shipLengths);
    }

    internal class BotShipLocation
    {
        public bool IsVertical { get; set; }
        public int ConstantRowColumn { get; set; }
        public int StartShepPoint { get; set; }
        public int ShipSize { get; set; }
    }
}