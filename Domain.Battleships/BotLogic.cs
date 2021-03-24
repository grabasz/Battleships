using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Battleships
{
    public class BotLogic
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
            var hits = GetHitsList();
            
            var bothSidesAvailable =  BothSidesAvailable(hits);
            if (IsShipLocationAlmostKnown(hits, bothSidesAvailable))
            {
                return TakeOneFromSides(hits);
            }
            if (IsShoutingNearTarget() && !bothSidesAvailable)
            {
                return ShootNearFirstHit();
            }

            return GetNextRandom();
        }

        private List<Coordinate> GetHitsList()
        {
            var coordinates = _alreadyGeneratedCoordinates.
                Where(x =>x.Value == Status.Hit).
                Select(x => x.Key).ToList();
            if (coordinates.GroupBy(x => x.Row).Count() < coordinates.Count())
                return coordinates.OrderBy(x => x.ColumnToIndex).ToList();
            return coordinates.OrderBy(x => x.RowToIndex).ToList();
        }

        private static bool IsShipLocationAlmostKnown(List<Coordinate> hits, bool bothSidesAvailable)
        {
            return hits.Count >= 2 && bothSidesAvailable;
        }

        private bool BothSidesAvailable(List<Coordinate> allHits)
        {
            if (allHits.Count < 2)
                return false;
            var onePossibleSide= TakeOneFromSides(allHits);
            return CoordinatsNotUsed(onePossibleSide);
        }

        private bool IsShoutingNearTarget()
        {
            return _alreadyGeneratedCoordinates.Any( x => x.Value == Status.Hit);
        }

        private Coordinate ShootNearFirstHit()
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

        public void StoreLastStatus(Coordinate coordinate, Status status)
        {
            _alreadyGeneratedCoordinates.Add(new KeyValuePair<Coordinate, Status>(coordinate,status));
        }

        private Coordinate GetNextRandom()
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

        public void MarkShipAsSunk(List<Coordinate> sunkShipCoordinates)
        {
            foreach (var sunk in sunkShipCoordinates)
            {
                var coordinateForMark =_alreadyGeneratedCoordinates.Single(x => x.Key.Equals(sunk));
                _alreadyGeneratedCoordinates.Remove(coordinateForMark);
                _alreadyGeneratedCoordinates.Add(
                    new KeyValuePair<Coordinate, Status>(coordinateForMark.Key, Status.ShipHasSunk));
            }
        }
    }
}