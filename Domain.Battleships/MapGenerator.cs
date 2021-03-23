using System.Collections.Generic;

namespace Domain.Battleships
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
                var randomShipLocation = new BotShipLocation
                {
                    ShipSize = shipSize,
                    IsVertical = _randomShipDataGenerator.GetIsVertical(),
                    ConstantRowColumn = _randomShipDataGenerator.GetRand0To9(),
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

        private static Ship PlaceShipVerticalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                map[i, botShipLocation.ConstantRowColumn] = true;
            }

            return CreateVerticalShip(botShipLocation);
        }

        private static Ship CreateHorizontalShip(BotShipLocation botShipLocation)
        {
            return new Ship
            {
                ShipFront = Coordinate.FromIndex(botShipLocation.ConstantRowColumn, botShipLocation.StartShepPoint),
                ShipBack = Coordinate.FromIndex(botShipLocation.ConstantRowColumn,
                    botShipLocation.StartShepPoint + botShipLocation.ShipSize - 1)
            };
        }

        private static Ship CreateVerticalShip(BotShipLocation botShipLocation)
        {
            return new Ship
            {
                ShipFront = Coordinate.FromIndex(botShipLocation.StartShepPoint, botShipLocation.ConstantRowColumn),
                ShipBack = Coordinate.FromIndex(botShipLocation.StartShepPoint + botShipLocation.ShipSize - 1,
                    botShipLocation.ConstantRowColumn)
            };
        }

        private static Ship PlaceShipHorizontalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                map[botShipLocation.ConstantRowColumn, i] = true;
            }

            return CreateHorizontalShip(botShipLocation);
        }

        private static bool CanInsertShip( bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                if (botShipLocation.IsVertical)
                {
                    if (map[i, botShipLocation.ConstantRowColumn])
                        return false;
                }
                else
                    return !map[botShipLocation.ConstantRowColumn, i];
                
            }

            return true;
        }
    }
}