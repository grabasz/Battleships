using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class MapGeneratorTest
    {
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
            var ships = mapGenerator.Generate(shipLengths);
            ships.Should().HaveCount(3);
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

            var ships = r.Generate(shipLengths);
            ships.Should().Contain(x => x.ShipFront.Row == "A" && x.ShipFront.Column == "1");
            ships.Should().Contain(x => x.ShipBack.Row == "E" && x.ShipFront.Column == "1");
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

            var ships = r.Generate(shipLengths);
            ships.Should().Contain(x => x.ShipFront.Row == "A" && x.ShipFront.Column == "1");
            ships.Should().Contain(x => x.ShipBack.Row == "A" && x.ShipBack.Column == "5");
        }
    }
}