using System.Collections.Generic;
using Domain.Battleships.GamePlay;
using Domain.Battleships.MapGeneraton;
using Domain.Battleships.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Domain.Battleships.Test
{
    public class BotLogicTest
    {
        [Test]
        public void ShouldUseRandomDataAtStart()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            botLogic.GetNextCoordinate();

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }

        [Test]
        public void ShouldUseNonRandomGeneratorIfPreviousWasHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);

            botLogic.GetNextCoordinate();

            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }

        [Test]
        public void ShouldCheckAllPossibleDirectionsUntilNextHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(3);

            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 2), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(0, 1), Status.Miss);

            botLogic.GetNextCoordinate();
            shipDataGeneratorMock.Verify(x => x.GetDirection(),Times.Once);
        }

        [Test]
        public void ShouldTargetOneOfBothSidesAfterFirstHit()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(3);
        }

        [Test]
        public void ShouldKeepOneDirectionUntilMiss()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Hit);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(4);
        }

        [Test]
        public void ShouldTurnToTheOtherSideWhenLastWasMiss()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Miss);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(0);
        }

        [Test]
        public void ShouldShootAgainNearFirstHitWhenBothSidesAreMissed_ItIsCornerCaseForTwoConnectedShips()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(1);
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(0, 1), Status.Miss);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(1);
            coordinate.ColumnToIndex.Should().Be(2);
        }

        [Test]
        public void ShouldGetSortedHitListForLookingForSides()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(1);
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(4, 1), Status.Miss);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(0);
        }

        [Test]
        public void ShouldGetRidOfWrongPath_CornerCaseWhen3ShipsAreSticked()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(1);
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 2), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2,3), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2,4), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2,0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(4, 1), Status.Miss);

            var coordinate = botLogic.GetNextCoordinate();
            coordinate.RowToIndex.Should().Be(0);
        }

        [Test]
        public void ShouldGetRidOfWrongPath_CornerCaseWhen2ShipsAreSticked()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(1);
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 5), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 5), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(4, 5), Status.ShipHasSunk);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 5), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 4), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 6), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 7), Status.Miss);




            var coordinate = botLogic.GetNextCoordinate();
            coordinate.ColumnToIndex.Should().Be(3);
        }

        [Test]
        public void ShouldMarkShipAsSunkForGivenCollectionNextPointWillBeRandomGenerated()
        {
            var shipDataGeneratorMock = new Mock<IShipDataGenerator>();
            shipDataGeneratorMock.Setup(m => m.GetDirection()).Returns(1);
            var botLogic = new BotLogic(shipDataGeneratorMock.Object);


            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 0), Status.Miss);
            botLogic.StoreLastStatus(Coordinate.FromIndex(1, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(2, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(3, 1), Status.Hit);
            botLogic.StoreLastStatus(Coordinate.FromIndex(0, 1), Status.Hit);


            var listForMarking = new List<Coordinate>
            {
                Coordinate.FromIndex(1, 0),
                Coordinate.FromIndex(1, 1),
                Coordinate.FromIndex(2, 1),
                Coordinate.FromIndex(3, 1),
                Coordinate.FromIndex(0, 1)
            };

            botLogic.MarkShipAsSunk(listForMarking);

            botLogic.GetNextCoordinate();
            shipDataGeneratorMock.Verify(x => x.GetRand0To9(), Times.Exactly(2));
        }
    }
}