﻿using System.Collections.Generic;

namespace Domain.Battleships
{
    public class GameRoom
    {
        public BotLogic BotLogic{ get; } = new BotLogic(new RandomShipDataGenerator());
        public List<(int, int)> AlreadyInsertedUsedFields { get; } = new List<(int, int)>();
        public Game UserGame { get; set; }
        public Game BotGame { get; set; }
    }
}