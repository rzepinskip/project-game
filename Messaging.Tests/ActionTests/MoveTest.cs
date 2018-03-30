using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;
using Xunit;

namespace Messaging.Tests.ActionTests
{
    /*
    public class MoveTest : ActionTest
    {
        [Fact]
        public void BasicMove()
        {
            SetTestedPlayerLocation(new Location(1, 3));

            var message = new MoveRequest(PlayerId, Direction.Right);
            if (message.Execute(GameMasterBoard) is MoveResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(2, 3), PlayerId);
        }

        [Fact]
        public void MoveOnEnemyGoalArea()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = new MoveRequest(PlayerId, Direction.Down);
            if (message.Execute(GameMasterBoard) is MoveResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(1, 2), PlayerId);
        }

        [Fact]
        public void MoveOnPiece()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = new MoveRequest(PlayerId, Direction.Right);
            if (message.Execute(GameMasterBoard) is MoveResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(2, 2), PlayerId);
            AssertPiece(new Location(2, 2), 2);
        }

        [Fact]
        public void MoveOnPlayer()
        {
            SetTestedPlayerLocation(new Location(1, 4));

            var message = new MoveRequest(PlayerId, Direction.Left);
            if (message.Execute(GameMasterBoard) is MoveResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(1, 4), PlayerId);
            AssertPlayerLocation(new Location(0, 4), OtherPlayerId);
        }

        [Fact]
        public void MoveOutsideBoard()
        {
            SetTestedPlayerLocation(new Location(0, 3));

            var message = new MoveRequest(PlayerId, Direction.Left);
            if (message.Execute(GameMasterBoard) is MoveResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(0, 3), PlayerId);
        }
    }
    */
}