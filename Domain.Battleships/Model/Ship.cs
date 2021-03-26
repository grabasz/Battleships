using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Battleships.Model
{
    public class Ship
    {
        private List<Coordinate> _notDestroyedPart;
        public Coordinate ShipFront { get; set; }
        public Coordinate ShipBack { get; set; }

        public List<Coordinate> NotDestroyedPart
        {
            get
            {
                if (_notDestroyedPart == null)
                    _notDestroyedPart = GetAllPoints();
                return _notDestroyedPart;
            }
            set => _notDestroyedPart = value;
        }

        public List<Coordinate> GetAllPoints()
        {
            if (ShipFront.Row == ShipBack.Row )
            {
                return GetCoordinatesForHorizontalShip().ToList();
            }

            if (ShipFront.Column == ShipBack.Column)
            {
                return GetCoordinatesForVerticalShip().ToList();
            }

            throw new Exception("Cannot get points for diagonal ship");
        }

        private static string IntToLetter(int i)
        {
            return ((char)(i + 64)).ToString();
        }

        private IEnumerable<Coordinate> GetCoordinatesForVerticalShip()
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
        }
    }
}