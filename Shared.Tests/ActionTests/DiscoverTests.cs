using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Shared.Tests.ActionTests
{
    public class DiscoverTests : ActionTest
    {
        private Discover GetDiscoverMessage()
        {
            return new Discover()
            {
                PlayerId = this.PlayerId,
            };

        }

        [Fact]
        public void BasicDiscover()
        {
            var location = new Location(2, 4);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            if (message.Execute(_gameMasterBoard) is DiscoverResponse response)
            {
                Assert.Equal(9, response.TaskFields.Count());
                response.Update(_playerBoard);
            }

        }


        [Fact]
        public void DiscoverPiece()
        {
            var location = new Location(1, 4);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            if (message.Execute(_gameMasterBoard) is DiscoverResponse response)
                response.Update(_playerBoard);

            AssertPiece(new Location(0, 5), 1);
        }

        [Fact]
        public void DiscoverPlayer()
        {
            var location = new Location(1, 4);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            if (message.Execute(_gameMasterBoard) is DiscoverResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(0, 4), 2);
        }

        [Fact]
        public void DiscoverOnBoardEdge()
        {
            var location = new Location(0,3);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            var response = message.Execute(_gameMasterBoard) as DiscoverResponse;
            Assert.Equal(6, response.TaskFields.Count());
        }

        [Fact]
        public void DiscoverOnGoalAreaEdge()
        {
            var location = new Location(1,6);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            var response = message.Execute(_gameMasterBoard) as DiscoverResponse;
            Assert.Equal(3, response.TaskFields.Count());
        }

        [Fact]
        public void DiscoverOnTaskAreaEdge()
        {
            var location = new Location(1, 5);
            SetTestedPlayerLocation(location);

            var message = GetDiscoverMessage();
            var response = message.Execute(_gameMasterBoard) as DiscoverResponse;
            Assert.Equal(6, response.TaskFields.Count());
        }
    }
}
