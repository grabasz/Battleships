using System.Collections.Generic;
using Domain.Battleships.MapGeneraton;
using Domain.Battleships.Model;

namespace Domain.Battleships.GamePlay
{
    public class GamePool
    {
        private static readonly List<int> ShipsSizes = new List<int>
        {
            5, 4, 4
        };

        private static int _lastGameId;
        public static Dictionary<int, GameRoom> Games { get; } = new Dictionary<int, GameRoom>();

        public static int CreateGame(List<Ship> coordinates)
        {
            _lastGameId++;
            var mapGenerator = new MapGenerator(new RandomShipDataGenerator());
            var botShips = mapGenerator.Generate(ShipsSizes);

            Games.Add(_lastGameId, new GameRoom
            {
                UserGame = new Game(coordinates),
                BotGame = new Game(botShips)
            });
            return _lastGameId;
        }
    }
}