using System.Collections.Generic;

namespace Domain.Battleships.Test
{
    public class MapGenerator : IMapGenerator
    {
        private readonly IShipDataGenerator _randomShipDataGenerator;

        public MapGenerator(IShipDataGenerator randomShipDataGenerator)
        {
            _randomShipDataGenerator = randomShipDataGenerator;
        }

        public bool[,] Generate(List<int> shipLengths)
        {
            var map = new bool[10, 10];
            
            foreach (var shipSize in shipLengths)
            {
                InsertShip(shipSize, map);
            }

            return map;
        }

        private void InsertShip( int shipSize, bool[,] map)
        {
            while (true)
            {
                var randomShipLocation = new BotShipLocation
                {
                    ShipSize = shipSize,
                    IsVertical = _randomShipDataGenerator.GetIsVertical(),
                    ConstantRowColumn = _randomShipDataGenerator.GetConstantRowColumn(),
                    StartShepPoint = _randomShipDataGenerator.GetStartShipPoint(shipSize)
                };

                if (CanInsertShip( map, randomShipLocation))
                {
                    if(randomShipLocation.IsVertical)
                        PlaceShipVerticalOnMap( map, randomShipLocation);
                    else
                        PlaceShipHorizontalOnMap(map, randomShipLocation);
                    break;
                }
            }
        }

        private static void PlaceShipVerticalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                map[i, botShipLocation.ConstantRowColumn] = true;
            }
        }

        private static void PlaceShipHorizontalOnMap(bool[,] map, BotShipLocation botShipLocation)
        {
            for (var i = botShipLocation.StartShepPoint; i < botShipLocation.ShipSize; i++)
            {
                map[botShipLocation.ConstantRowColumn, i] = true;
            }
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
                else if (map[botShipLocation.ConstantRowColumn, i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}