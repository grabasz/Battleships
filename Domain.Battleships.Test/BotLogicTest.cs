using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class BotLogicTest
    {
        [Test]
        public void ShouldUseRandomDataAtStart()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            botLogic.GetNextCoordinate();

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }

        [Test]
        public void ShouldUseNonRandomGeneratorIfPreviousWasHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));

            botLogic.GetNextCoordinate();

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }

        [Test]
        public void ShouldCheckAllPossibleDirectionsUntilNextHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(3);

            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 2), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(0, 1), Status.Miss));

            botLogic.GetNextCoordinate();
            shipDataGeneratorMock.Verify(x => x.GetDirection(),Times.Once);
        }

        [Test]
        public void ShouldKeepOneDirectionUntilMissOrShipHasSunk()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Hit));

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(3);
        }
    }

    internal class BotLogic
    {
        private readonly List<KeyValuePair<Coordinate, Status>> _alreadyGeneratedCoordinates =
            new List<KeyValuePair<Coordinate, Status>>();

        private readonly IShipDataGenerator _shipDataGenerator;

        public BotLogic(IShipDataGenerator shipDataGenerator)
        {
            _shipDataGenerator = shipDataGenerator;
        }

        public Coordinate GetNextCoordinate()
        {
            var isShoutingNearTarget = _alreadyGeneratedCoordinates.Select(x => x.Value).Contains( Status.Hit);

            var hits = _alreadyGeneratedCoordinates.Where(x =>x.Value == Status.Hit).ToList();
            if (hits.Count == 2 /*both sides are avail*/)
            {
                return TakeOneFromSides(hits.Select(x => x.Key));
            }
            if ( isShoutingNearTarget)
            {
                var firstHit= _alreadyGeneratedCoordinates.First(x => x.Value == Status.Hit);
                var coordinate = firstHit.Key;

                while (true)
                {
                    var c = GetRandomDirectionPair(coordinate.RowToIndex, coordinate.ColumnToIndex);
                    if (CoordinatsNotUsed(c))
                        return c;
                }
            }

            return GetNextRandomPair();
        }

        private bool CoordinatsNotUsed(Coordinate c)
        {
            return _alreadyGeneratedCoordinates.All(x => x.Key != c);
        }

        private Coordinate TakeOneFromSides(IEnumerable<Coordinate> coordinates)
        {
            var firstHit = coordinates.First();
            var secondHit = coordinates.Last();

            if (firstHit.Row == secondHit.Row)
            {
                var max = Math.Max(firstHit.ColumnToIndex, secondHit.ColumnToIndex);
                return Coordinate.FromIndex(firstHit.RowToIndex, max + 1);
            }

            else
            {
                var max = Math.Max(firstHit.RowToIndex, secondHit.RowToIndex);
                return Coordinate.FromIndex(max + 1, firstHit.ColumnToIndex);
            }
        }

        private Coordinate GetRandomDirectionPair(int row, int column)
        {
            var direction = _shipDataGenerator.GetDirection();

            if (direction == 0)
                return Coordinate.FromIndex(row + 1, column);
            if (direction == 1)
                return Coordinate.FromIndex(row, column + 1);
            if (direction == 2)
                return Coordinate.FromIndex(row - 1, column);

            return Coordinate.FromIndex(row, column - 1);
        }

        public void StoreLastStatus(KeyValuePair<Coordinate, Status> pair)
        {
            _alreadyGeneratedCoordinates.Add(pair);
        }

        private Coordinate GetNextRandomPair()
        {
            while (true)
            {
                var randomRow = _shipDataGenerator.GetRand0To9();
                var randomColumn = _shipDataGenerator.GetRand0To9();

                if (!_alreadyGeneratedCoordinates.Select(x => x.Key)
                    .Contains(Coordinate.FromIndex(randomRow, randomColumn)))
                    return Coordinate.FromIndex(randomRow, randomColumn);
            }
        }
    }
}