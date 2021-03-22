using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Battleships;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipAPI.SignalRHubs
{
    public class InitializationGameHub : Hub
    {
        public async Task Play(int gameId, int row, int column)
        {
            var gameRoom = GamePool.Games[gameId];
            var coordinateFromStr = Coordinate.FromIndex(row,column);
            var status = gameRoom.BotChecker.Play(coordinateFromStr);
            if (gameRoom.BotChecker.IsGameOver())
            {
                await Clients.Caller.SendAsync("gameWon", gameId);
                return;
            }
                
            await Clients.Caller.SendAsync("fieldStatus", row, 
                column,
                status);
        }

        public async Task InitGame(List<List<string>> playerCoordinates)
        {
            var playerShips = playerCoordinates.Select(x => new Ship
            {
                ShipFront = Coordinate.FromSingleString(x[0]),
                ShipBack = Coordinate.FromSingleString(x[1])
            });

            var gameId = GamePool.CreateGame(playerShips);
            await Clients.Caller.SendAsync("gameReadyRequest", gameId);
        }
    }

    public class GamePool
    {
        private static readonly List<int> ShipsSizes = new List<int>
        {
            5, 4, 4
        };

        private static int _lastGameId;
        public static Dictionary<int, GameRoom> Games { get; } = new Dictionary<int, GameRoom>();

        public static int CreateGame(IEnumerable<Ship> coordinates)
        {
            _lastGameId++;
            var mapGenerator = new MapGenerator(new RandomShipDataGenerator());
            var botShips = mapGenerator.Generate(ShipsSizes);

            Games.Add(_lastGameId, new GameRoom
            {
                UserChecker = new GameChecker(coordinates),
                BotChecker = new GameChecker(botShips)
            });
            return _lastGameId;
        }
    }
}