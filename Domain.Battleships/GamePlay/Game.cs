using System.Collections.Generic;
using System.Linq;
using Domain.Battleships.Model;

namespace Domain.Battleships.GamePlay
{
    public interface IGame
    {
        Status Play(Coordinate coordinate);
        List<Coordinate> GetShipByCoordinate(Coordinate playerCoordinate);
        bool IsGameOver();
    }

    public class Game : IGame
    {
        private readonly List<Ship> _fleet;

        public Game(List<Ship> fleet)
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
            ship.NotDestroyedPart = ship.NotDestroyedPart.Where(x => !Equals(x, coordinate)).ToList();
        }

        public List<Coordinate> GetShipByCoordinate(Coordinate playerCoordinate)
        {
            return GetShip(playerCoordinate).GetAllPoints();
        }

        private Ship GetShip(Coordinate coordinate)
        {
            return _fleet.FirstOrDefault(x => x.GetAllPoints().Contains(coordinate));
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