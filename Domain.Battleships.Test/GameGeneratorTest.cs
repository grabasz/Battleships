using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameGeneratorTest
    {
        [Test]
        public void ShouldCreate10x10Map()
        {
            GameGenerator r = new GameGenerator(new RandomShipDataGenerator());
            bool[,] map = r.Generate(new List<int>());
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
        GameGenerator r = new GameGenerator(new RandomShipDataGenerator());
            bool[,] map = r.Generate(shipLengths);
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
            GameGenerator r = new GameGenerator(shipDataGeneratorMock.Object);

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
            GameGenerator r = new GameGenerator(shipDataGeneratorMock.Object);

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

    public class RandomShipDataGenerator : IShipDataGenerator
    {
        private readonly Random _random = new Random();

        public int GetStartShipPoint(int shipSize)
        {
            return _random.Next(0, shipSize - 1);
        }

        public int GetConstantRowColumn()
        {
            return _random.Next(0, 9);
        }

        public bool GetIsVertical()
        {
            return _random.Next(0, 1).Equals(1);
        }
    }

    public class GameGenerator
    {
        private readonly IShipDataGenerator _randomShipDataGenerator;

        public GameGenerator(IShipDataGenerator randomShipDataGenerator)
        {
            _randomShipDataGenerator = randomShipDataGenerator;
        }

        public bool[,] Generate(List<int> shipLengths)
        {
            var map = new bool[10, 10];
            
            foreach (var shipSize in shipLengths)
            {
                InsertShip(shipSize, map);
            }

            return map;
        }

        private void InsertShip( int shipSize, bool[,] map)
        {
            while (true)
            {
                var randomShipLocation = new BotShipLocation
                {
                    ShipSize = shipSize,
                    IsVertical = _randomShipDataGenerator.GetIsVertical(),
                    ConstantRowColumn = _randomShipDataGenerator.GetConstantRowColumn(),
                    StartShepPoint = _randomShipDataGenerator.GetStartShipPoint(shipSize)
            };

                if (CanInsertShip( map, randomShipLocation))
                {
                    if(randomShipLocation.IsVertical)
                        PlaceShipVerticalOnMap( map, randomShipLocation);
                    else
                        PlaceShipHorizontalOnMap(map, randomShipLocation);
                    break;
                }
            }
        }

        private static void PlaceShipVerticalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                    map[i, botShipLocation.ConstantRowColumn] = true;
            }
        }

        private static void PlaceShipHorizontalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                    map[botShipLocation.ConstantRowColumn, i] = true;
            }
        }

        private static bool CanInsertShip( bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                if (botShipLocation.IsVertical)
                {
                    if (map[i, botShipLocation.ConstantRowColumn])
                        return false;
                }
                else if (map[botShipLocation.ConstantRowColumn, i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    internal class BotShipLocation
    {
        public bool IsVertical { get; set; }
        public int ConstantRowColumn { get; set; }
        public int StartShepPoint { get; set; }
        public int ShipSize { get; set; }
    }
}