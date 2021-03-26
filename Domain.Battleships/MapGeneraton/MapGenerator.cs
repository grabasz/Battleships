using System.Collections.Generic;
using System.Linq;
using Domain.Battleships.Model;

namespace Domain.Battleships.MapGeneraton
{
    public class MapGenerator : IMapGenerator
    {
        private readonly IShipDataGenerator _randomShipDataGenerator;

        public MapGenerator(IShipDataGenerator randomShipDataGenerator)
        {
            _randomShipDataGenerator = randomShipDataGenerator;
        }

        public List<Ship> Generate(List<int> shipLengths)
        {
            var map = new bool[10, 10];
            List<Ship> insertedShips = new List<Ship>();
            
            foreach (var shipSize in shipLengths)
            {
                insertedShips.Add(InsertShip(shipSize, insertedShips));
            }

            return insertedShips;
        }

        private Ship InsertShip( int shipSize, List<Ship> map)
        {
            while (true)
            {
                var randomShipLocation = new RandomShipLocation
                {
                    ShipSize = shipSize,
                    IsVertical = _randomShipDataGenerator.GetIsVertical(),
                    ConstantRowOrColumn = _randomShipDataGenerator.GetRand0To9(),
                    StartShepPoint = _randomShipDataGenerator.GetStartShipPoint(shipSize)
                };
                var shipForInsert= GetShip(randomShipLocation);
                if (CanInsertShip( map, shipForInsert))
                {
                    return shipForInsert;
                }
            }
        }

        private static Ship GetShip(RandomShipLocation randomShipLocation)
        {
            if (randomShipLocation.IsVertical)
                return PlaceShipVerticalOnMap(randomShipLocation);
            return PlaceShipHorizontalOnMap(randomShipLocation);
        }

        private static Ship PlaceShipVerticalOnMap(RandomShipLocation randomShipLocation)
        {
            return CreateVerticalShip(randomShipLocation);
        }

        private static Ship CreateHorizontalShip(RandomShipLocation randomShipLocation)
        {
            return new Ship
            {
                ShipFront = Coordinate.FromIndex(randomShipLocation.ConstantRowOrColumn, randomShipLocation.StartShepPoint),
                ShipBack = Coordinate.FromIndex(randomShipLocation.ConstantRowOrColumn,
                    randomShipLocation.StartShepPoint + randomShipLocation.ShipSize - 1)
            };
        }

        private static Ship CreateVerticalShip(RandomShipLocation randomShipLocation)
        {
            return new Ship
            {
                ShipFront = Coordinate.FromIndex(randomShipLocation.StartShepPoint, randomShipLocation.ConstantRowOrColumn),
                ShipBack = Coordinate.FromIndex(randomShipLocation.StartShepPoint + randomShipLocation.ShipSize - 1,
                    randomShipLocation.ConstantRowOrColumn)
            };
        }

        private static Ship PlaceShipHorizontalOnMap(RandomShipLocation randomShipLocation)
        {
            return CreateHorizontalShip(randomShipLocation);
        }

        private static bool CanInsertShip( List<Ship> alreadyInsertedShips, Ship shipForInsert)
        {
            var allOccupiedPoints = alreadyInsertedShips.SelectMany(x => x.GetAllPoints());
            return allOccupiedPoints.All(x => !shipForInsert.GetAllPoints().Contains(x));
        }
    }
}