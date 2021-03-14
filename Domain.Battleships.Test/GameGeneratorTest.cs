using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class GameGeneratorTest
    {
        [Test]
        public void ShouldCreate10x10Map()
        {
            GameGenerator r = new GameGenerator();
            bool[,] map = r.Generate(new List<int>());
            map.GetLongLength(0).Should().Be(10);
            map.GetLongLength(1).Should().Be(10);
        }

        [Test]
        [Repeat(100)]
        public void ShouldInitializeMapShip100TimesToVerifyIfRandomCanCauseTheError()
        {
        List<int> ShipLengths = new List<int>
        {
            5,
            4,
            4
        };
        GameGenerator r = new GameGenerator();
            bool[,] map = r.Generate(ShipLengths);
            map.Should().Contain(true);
        }
    }

    public class GameGenerator
    {
        public bool[,] Generate(List<int> shipLengths)
        {
            var map = new bool[10, 10];
            Random r = new Random();
            foreach (var shipSize in shipLengths)
            {
                InsertShip(r, shipSize, map);
            }

            return map;
        }

        private static void InsertShip(Random r, int shipSize, bool[,] map)
        {
            while (true)
            {
                var isVertical = r.Next(0, 1).Equals(1);
                var randomValue = r.Next(0, 9);
                var verticalHorizontalRandom = r.Next(0, shipSize - 1);

                if (CanInsertShip(shipSize, map, verticalHorizontalRandom, isVertical, randomValue))
                {
                    PlaceShipOnMap(shipSize, map, verticalHorizontalRandom, isVertical, randomValue);
                    break;
                }
            }
        }

        private static void PlaceShipOnMap(int shipSize, bool[,] map, int verticalHorizontalRandom, bool isVertical, int randomValue)
        {
            for (var i = verticalHorizontalRandom; i < shipSize; i++)
            {
                if (isVertical)
                    map[i, randomValue] = true;
                else
                    map[randomValue, i] = true;
            }
        }

        private static bool CanInsertShip(int shipSize, bool[,] map, int verticalHorizontalRandom, bool isVertical,
            int randomValue)
        {
            for (var i = verticalHorizontalRandom; i < shipSize; i++)
            {
                if (isVertical)
                {
                    if (map[i, randomValue])
                        return false;
                }
                else if (map[randomValue, i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}