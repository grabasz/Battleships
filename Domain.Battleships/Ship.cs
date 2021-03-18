using System;
using System.Collections.Generic;

namespace Domain.Battleships
{
    public class Ship
    {
        private IEnumerable<Coordinate> _notDestroyedPart;
        public Coordinate ShipFront { get; set; }
        public Coordinate ShipBack { get; set; }

        public IEnumerable<Coordinate> NotDestroyedPart
        {
            get
            {
                if (_notDestroyedPart == null)
                    _notDestroyedPart = GetAllPoints();
                return _notDestroyedPart;
            }
            set { _notDestroyedPart = value; }
        }

        public IEnumerable<Coordinate> GetAllPoints()
        {
            if (ShipFront.Row == ShipBack.Row )
            {
                return GetCoordinatesForHorizontalShip();
            }

            if (ShipFront.Column == ShipBack.Column)
            {
                return GetCoodinatesForVerticalShip();
            }

            throw new Exception("Cannot get points for diagonal ship");
        }

        private IEnumerable<Coordinate> GetCoodinatesForVerticalShip()
        {
            var front = ShipFront.RowToIndex + 1;
            var back = ShipBack.RowToIndex + 1;
            var max = Math.Max(front, back);
            var min = Math.Min(front, back);

            for (var i = min; i <= max; i++)
            {
                yield return new Coordinate(IntToLetter(i), ShipFront.Column);
            }
        }

        private static string IntToLetter(int i)
        {
            return ((char)(i + 64)).ToString();
        }

        private IEnumerable<Coordinate> GetCoordinatesForHorizontalShip()
        {
            var front = ShipFront.ColumnToIndex + 1;
            var back = ShipBack.ColumnToIndex + 1;
            var max = Math.Max(front, back);
            var min = Math.Min(front, back);

            for (var i = min; i <= max; i++)
            {
                yield return new Coordinate(ShipFront.Row, i.ToString());
            }

            yield break;
        }
    }
}