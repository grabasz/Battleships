using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace Domain.Battleships.Test
{
    public class BotLogicTest
    {
        [Test]
        public void ShouldUseRandomDataAtStart()
        {
            Mock<IShipDataGenerator> shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            BotLogic botLogic = new BotLogic(shipDataGeneratorMock.Object);
            Status status = Status.Miss;
            Coordinate coordinate = botLogic.GetNextCoordinate(status);

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(),Times.Exactly(2));
        }

        [Test]
        public void ShouldUseNonRandomGeneratorIfPreviousWasHit()
        {
            Mock<IShipDataGenerator> shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            BotLogic botLogic = new BotLogic(shipDataGeneratorMock.Object);
            Status status = Status.Hit;
            botLogic.GetNextCoordinate(status);
            Coordinate coordinate = botLogic.GetNextCoordinate(status);

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }
    }

    internal class BotLogic
    {
        private readonly IShipDataGenerator _shipDataGenerator;
        readonly Random _random = new Random();
        private Coordinate _lastCoordinate;

        public BotLogic(IShipDataGenerator shipDataGenerator)
        {
            _shipDataGenerator = shipDataGenerator;
        }

        private List<(int, int)> AlreadyInsertedUsedFields { get; } = new List<(int, int)>();

        public Coordinate GetNextCoordinate(Status previousStatus)
        {
            if (previousStatus == Status.Hit)
            {

            }
            {
                _lastCoordinate = GetNextRandomPair();
                return _lastCoordinate;
            }
        }

        private Coordinate GetNextRandomPair()
        {
            while (true)
            {
                var randomRow = _shipDataGenerator.GetRand0To9();
                var randomColumn = _shipDataGenerator.GetRand0To9();
                if (!AlreadyInsertedUsedFields.Contains((randomRow, randomColumn)))
                {
                    var rawColumn = (randomRow, randomColumn);
                    AlreadyInsertedUsedFields.Add(rawColumn);
                    return Coordinate.FromIndex(randomRow, randomColumn);
                }
            }
        }
    }
}