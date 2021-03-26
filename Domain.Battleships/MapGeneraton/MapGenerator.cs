using System.Collections.Generic;
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
                insertedShips.Add(InsertShip(shipSize, map));
            }

            return insertedShips;
        }

        private Ship InsertShip( int shipSize, bool[,] map)
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

                if (CanInsertShip( map, randomShipLocation))
                {
                    if(randomShipLocation.IsVertical)
                        return  PlaceShipVerticalOnMap( map, randomShipLocation);
                    return PlaceShipHorizontalOnMap(map, randomShipLocation);
                }
            }
        }

        private static Ship PlaceShipVerticalOnMap(bool[,] map, RandomShipLocation randomShipLocation)
        {
            for (var i = randomShipLocation.StartShepPoint; i < randomShipLocation.ShipSize; i++)
            {
                map[i, randomShipLocation.ConstantRowOrColumn] = true;
            }

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

        private static Ship PlaceShipHorizontalOnMap(bool[,] map, RandomShipLocation randomShipLocation)
        {
            for (var i = randomShipLocation.StartShepPoint; i < randomShipLocation.ShipSize; i++)
            {
                map[randomShipLocation.ConstantRowOrColumn, i] = true;
            }

            return CreateHorizontalShip(randomShipLocation);
        }

        private static bool CanInsertShip( bool[,] map, RandomShipLocation randomShipLocation)
        {
            for (var i = randomShipLocation.StartShepPoint; i < randomShipLocation.ShipSize; i++)
            {
                if (randomShipLocation.IsVertical)
                {
                    if (map[i, randomShipLocation.ConstantRowOrColumn])
                        return false;
                }
                else
                    return !map[randomShipLocation.ConstantRowOrColumn, i];
                
            }

            return true;
        }
    }
}