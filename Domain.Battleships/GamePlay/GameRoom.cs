using Domain.Battleships.MapGeneraton;

namespace Domain.Battleships.GamePlay
{
    public class GameRoom
    {
        public BotLogic BotLogic{ get; } = new BotLogic(new RandomShipDataGenerator());
        public Game UserGame { get; set; }
        public Game BotGame { get; set; }
    }
}