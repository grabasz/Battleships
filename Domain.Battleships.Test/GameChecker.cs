using System.Collections.Generic;
using System.Linq;

namespace Domain.Battleships.Test
{
    public class GameChecker
    {
        private readonly List<Ship> _fleet;

        public GameChecker(List<Ship> fleet)
        {
            _fleet = fleet;
        }

        public Status Play(Coordinate coordinate)
        {
            if (IsAShip(coordinate))
            {
                var ship = GetShip(coordinate);
                DestroyPartOfShip(ship,coordinate);
                if(ship.NotDestroyedPart.Any())
                    return Status.Hit;
                return Status.ShipHasSunk;
            }

            return Status.Miss;
        }

        private void DestroyPartOfShip(Ship ship, Coordinate coordinate)
        {
            ship.NotDestroyedPart = ship.NotDestroyedPart.Where(x => !Equals(x, coordinate));
        }

        private Ship GetShip(Coordinate coordinate)
        {
            return _fleet.FirstOrDefault(x => x.NotDestroyedPart.Contains(coordinate));
        }

        private bool IsAShip(Coordinate coordinate)
        {
            return _fleet.Any(x => x.NotDestroyedPart.Contains(coordinate));
        }

        public bool IsGameOver()
        {
            return _fleet.All(x => !x.NotDestroyedPart.Any());
        }
    }
}