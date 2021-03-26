using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Battleships;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BattleShipAPI.SignalRHubs
{
    public class InitializationGameHub : Hub
    public class GameHub : Hub
    {
        public async Task Play(int gameId, int row, int column)
        {
            var gameRoom = GamePool.Games[gameId];
            await PlayUserTurn(row, column, gameRoom);

            await PlayBotTurn( gameRoom);
        }

        private async Task PlayUserTurn(int row, int column, GameRoom gameRoom)
        {
            var playerCoordinate = Coordinate.FromIndex(row, column);
            var gameRoomBotGame = gameRoom.BotGame;

            var status = gameRoomBotGame.Play(playerCoordinate);

            if (status == Status.ShipHasSunk)
            {
                await ShipHasSunk(gameRoomBotGame.GetShipByCoordinate(playerCoordinate), "playerFieldStatus");
            }

            await Clients.Caller.SendAsync("playerFieldStatus", row,
                column,
                status);

            await GameOver(gameRoomBotGame, "gameWon");
        }

        private async Task PlayBotTurn( GameRoom gameRoom)
        {
            var gameRoomBotLogic = gameRoom.BotLogic;
            var nextCoordinate = gameRoomBotLogic.GetNextCoordinate();
            var gameRoomUserGame = gameRoom.UserGame;

            var status = gameRoomUserGame.Play(nextCoordinate);

            await Clients.Caller.SendAsync("opponentFieldStatus", nextCoordinate.RowToIndex,
                nextCoordinate.ColumnToIndex,
                status);

            gameRoomBotLogic.StoreLastStatus(nextCoordinate, status);

            if (status == Status.ShipHasSunk)
            {
                var shipByCoordinate = gameRoomUserGame.GetShipByCoordinate(nextCoordinate);
                gameRoomBotLogic.MarkShipAsSunk(shipByCoordinate);
                await ShipHasSunk(shipByCoordinate, "opponentFieldStatus");
            }
            

            await GameOver(gameRoomUserGame, "gameFail");
        }

        private async Task GameOver(Game gameRoomBotGame, string gameOverMethod)
        {
            if (gameRoomBotGame.IsGameOver())
            {
                await Clients.Caller.SendAsync(gameOverMethod);
            }
        }

        private async Task ShipHasSunk(List<Coordinate> getShipByCoordinate, string playerfieldstatus)
        {
                foreach (var point in getShipByCoordinate)
                    await Clients.Caller.SendAsync(playerfieldstatus, point.RowToIndex,
                        point.ColumnToIndex,
                        Status.ShipHasSunk);
        }

        public async Task InitGame(List<List<string>> playerCoordinates)
        {
            var playerShips = CreatePlayerShips(playerCoordinates);

            var gameId = GamePool.CreateGame(playerShips);
            await Clients.Caller.SendAsync("gameReadyRequest", gameId);
        }

        private static List<Ship> CreatePlayerShips(List<List<string>> playerCoordinates)
        {
            return playerCoordinates.Select(x => new Ship
            {
                ShipFront = Coordinate.FromSingleString(x[0]),
                ShipBack = Coordinate.FromSingleString(x[1])
            }).ToList();
        }
    }
}