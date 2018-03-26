using System.Collections.Generic;
using System.Linq;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;
using Xunit;

namespace Messaging.Tests.ActionTests
{
    /*
    public class DiscoverTests : ActionTest
    {
        private void AssertTaskFields(IEnumerable<TaskField> taskfields)
        {
            foreach (var field in taskfields) Assert.Equal(GameMasterBoard[field], field);
        }

        [Fact]
        public void BasicDiscover()
        {
            var location = new Location(2, 4);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            if (message.Execute(GameMasterBoard) is DiscoverResponse response)
            {
                Assert.Equal(9, response.TaskFields.Count());
                AssertTaskFields(response.TaskFields);

                response.Update(PlayerBoard);
            }
        }

        [Fact]
        public void DiscoverOnBoardEdge()
        {
            var location = new Location(0, 3);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Equal(6, response.TaskFields.Count());
            AssertTaskFields(response.TaskFields);
        }

        [Fact]
        public void DiscoverOnGoalArea()
        {
            var location = new Location(1, 7);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Empty(response.TaskFields);
        }

        [Fact]
        public void DiscoverOnGoalAreaCorner()
        {
            var location = new Location(0, 6);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Equal(2, response.TaskFields.Count());
            AssertTaskFields(response.TaskFields);
        }

        [Fact]
        public void DiscoverOnGoalAreaEdge()
        {
            var location = new Location(1, 6);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Equal(3, response.TaskFields.Count());
            AssertTaskFields(response.TaskFields);
        }

        [Fact]
        public void DiscoverOnTaskAreaCorner()
        {
            var location = new Location(0, 2);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Equal(4, response.TaskFields.Count());
            AssertTaskFields(response.TaskFields);
        }

        [Fact]
        public void DiscoverOnTaskAreaEdge()
        {
            var location = new Location(1, 5);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            var response = message.Execute(GameMasterBoard) as DiscoverResponse;

            Assert.Equal(6, response.TaskFields.Count());
            AssertTaskFields(response.TaskFields);
        }


        [Fact]
        public void DiscoverPiece()
        {
            var location = new Location(1, 4);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            if (message.Execute(GameMasterBoard) is DiscoverResponse response)
                response.Update(PlayerBoard);

            AssertPiece(new Location(0, 5), 1);
        }

        [Fact]
        public void DiscoverPlayer()
        {
            var location = new Location(1, 4);
            SetTestedPlayerLocation(location);

            var message = new DiscoverRequest(PlayerId);
            if (message.Execute(GameMasterBoard) is DiscoverResponse response)
                response.Update(PlayerBoard);

            AssertPlayerLocation(new Location(0, 4), 2);
        }
    }
    */
}