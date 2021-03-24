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

            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 2), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(0, 1), Status.Miss));

            botLogic.GetNextCoordinate();
            shipDataGeneratorMock.Verify(x => x.GetDirection(),Times.Once);
        }

        [Test]
        public void ShouldTargetOneOfBothSidesAfterFirstHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Hit));

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(3);
        }

        [Test]
        public void ShouldKeepOneDirectionUntilMiss()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(3, 1), Status.Hit));

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(4);
        }

        [Test]
        public void ShouldTurnToTheOtherSideWhenLastWasMiss()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 0), Status.Miss));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(1, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(2, 1), Status.Hit));
            botLogic.StoreLastStatus(new KeyValuePair<Coordinate, Status>(Coordinate.FromIndex(3, 1), Status.Miss));

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(0);

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
            var hits = _alreadyGeneratedCoordinates.Where(x =>x.Value == Status.Hit).ToList();
            if (hits.Count >= 2 /*both sides are avail*/)
            {
                return TakeOneFromSides(hits.Select(x => x.Key).ToList());
            }
            if (IsShoutingNearTarget())
            {
                return ShootNearLastHit();
            }

            return GetNextRandomPair();
        }

        private bool IsShoutingNearTarget()
        {
            return _alreadyGeneratedCoordinates.Any( x => x.Value == Status.Hit);
        }

        private Coordinate ShootNearLastHit()
        {
            var firstHit = _alreadyGeneratedCoordinates.First(x => x.Value == Status.Hit);
            var coordinate = firstHit.Key;

            while (true)
            {
                var c = GetRandomDirectionPair(coordinate.RowToIndex, coordinate.ColumnToIndex);
                if (CoordinatsNotUsed(c))
                    return c;
            }
        }

        private bool CoordinatsNotUsed(Coordinate c)
        {
            return _alreadyGeneratedCoordinates.All(x => !x.Key.Equals(c) );
        }

        private Coordinate TakeOneFromSides(List<Coordinate> coordinates)
        {
            var firstHit = coordinates.First();
            var secondHit = coordinates.Last();

            if (IsHorizontalShip(firstHit, secondHit))
            {
                return GetOneSideOfHorizontalShip(firstHit, secondHit);
            }

            return GetOneSideOfVerticalShip(firstHit, secondHit);
        }

        private static bool IsHorizontalShip(Coordinate firstHit, Coordinate secondHit)
        {
            return firstHit.Row == secondHit.Row;
        }

        private Coordinate GetOneSideOfVerticalShip(Coordinate firstHit, Coordinate secondHit)
        {
            var nextRow = Coordinate.FromIndex(GetNextValue(firstHit.RowToIndex,
                    secondHit.RowToIndex),
                firstHit.ColumnToIndex);
            if (CoordinatsNotUsed(nextRow))
                return nextRow;

            return Coordinate.FromIndex(GetPreviousValue(firstHit.RowToIndex,
                    secondHit.RowToIndex),
                firstHit.ColumnToIndex);
        }

        private Coordinate GetOneSideOfHorizontalShip(Coordinate firstHit, Coordinate secondHit)
        {
            var nextColumn = Coordinate.FromIndex(firstHit.RowToIndex,
                GetNextValue(firstHit.ColumnToIndex,
                    secondHit.ColumnToIndex));
            if (CoordinatsNotUsed(nextColumn))
                return nextColumn;
            return Coordinate.FromIndex(firstHit.RowToIndex,
                GetPreviousValue(firstHit.ColumnToIndex,
                    secondHit.ColumnToIndex));
        }

        private static int GetNextValue(int first, int second)
        {
            var max = Math.Max(first, second);
            return max + 1;
        }

        private static int GetPreviousValue(int first, int second)
        {
            var min = Math.Min(first, second);
            return min - 1;
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