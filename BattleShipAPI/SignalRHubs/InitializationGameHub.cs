using System;
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

            await PlayUserTurn(row, column, gameRoom);

            var rawColumn =GetNextRandomPair(gameRoom);

            await PlayBotTurn(rawColumn.row, rawColumn.column, gameRoom);
        }

        private (int row, int column) GetNextRandomPair(GameRoom gameRoom)
        {
            Random random = new Random();
            while (true)
            {
                var randomRow = random.Next(0, 10);
                var randomColumn = random.Next(0, 10);
                if (!gameRoom.AlreadyInsertedUsedFields.Contains((randomRow, randomColumn)))
                {
                    var rawColumn = (randomRow, randomColumn);
                    gameRoom.AlreadyInsertedUsedFields.Add(rawColumn);
                    return rawColumn;
                }
            }
        }

        private async Task PlayUserTurn(int row, int column, GameRoom gameRoom)
        {
            var playerCoordinate = Coordinate.FromIndex(row, column);
            var gameRoomBotGame = gameRoom.BotGame;

            var status = gameRoomBotGame.Play(playerCoordinate);

            await ShipHasSunk(status, gameRoomBotGame, playerCoordinate, "playerFieldStatus");

            await Clients.Caller.SendAsync("playerFieldStatus", row,
                column,
                status);

            await GameOver(gameRoomBotGame, "gameWon");
        }

        private async Task PlayBotTurn(int randomRow, int randomColumn, GameRoom gameRoom)
        {
            var opponentCoordinates = Coordinate.FromIndex(randomRow, randomColumn);
            var gameRoomUserGame = gameRoom.UserGame;

            var botStatus = gameRoomUserGame.Play(opponentCoordinates);
            await Clients.Caller.SendAsync("opponentFieldStatus", randomRow,
                randomColumn,
                botStatus);

            await ShipHasSunk(botStatus, gameRoomUserGame, opponentCoordinates, "opponentFieldStatus");
            await GameOver(gameRoomUserGame, "gameFail");
        }

        private async Task GameOver(Game gameRoomBotGame, string gameOverMethod)
        {
            if (gameRoomBotGame.IsGameOver())
            {
                await Clients.Caller.SendAsync(gameOverMethod);
            }
        }

        private async Task ShipHasSunk(Status status, Game gameRoomBotGame, Coordinate playerCoordinate, string playerfieldstatus)
        {
            if (status == Status.ShipHasSunk)
            {
                foreach (var point in gameRoomBotGame.GetShip(playerCoordinate).GetAllPoints())
                    await Clients.Caller.SendAsync(playerfieldstatus, point.RowToIndex,
                        point.ColumnToIndex,
                        Status.ShipHasSunk);
            }
        }

        public async Task InitGame(List<List<string>> playerCoordinates)
        {
            var playerShips = playerCoordinates.Select(x => new Ship
            {
                ShipFront = Coordinate.FromSingleString(x[0]),
                ShipBack = Coordinate.FromSingleString(x[1])
            }).ToList();

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