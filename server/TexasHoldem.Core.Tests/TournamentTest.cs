using System;
using System.Collections.Generic;
using System.Text;
using Darkhood.TexasHoldem.Core;
using Xunit;

namespace Darkhood.TexasHoldem.Core.Tests
{
    public class TournamentTest
    {
        [Fact]
        public void TestGameScenario1()
        {
            Player p1 = new Player(1, "Player 1");
            Player p2 = new Player(2, "Player 2");
            Player p3 = new Player(3, "Player 3");

            Tournament tournament = new Tournament();

            tournament.AddPlayer(p1);
            tournament.AddPlayer(p2);
            tournament.AddPlayer(p3);

            Assert.Equal(3, tournament.Players.Count);

            // Pre-Flop
            tournament.NextRound();

            Player nextTurnPlayer = null;

            nextTurnPlayer = tournament.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(nextTurnPlayer, p1);
            tournament.Check(nextTurnPlayer.PlayerId);

            nextTurnPlayer = tournament.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(nextTurnPlayer, p2);
            tournament.Check(nextTurnPlayer.PlayerId);

            nextTurnPlayer = tournament.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(nextTurnPlayer, p3);
            tournament.Check(nextTurnPlayer.PlayerId);

            // At this point it should be null
            //nextTurnPlayer = tournament.GetNextTurnPlayer();
            //Assert.Null(nextTurnPlayer);
        }
    }
}
