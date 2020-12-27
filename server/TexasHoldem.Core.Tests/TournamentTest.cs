using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Darkhood.TexasHoldem.Core.Tests
{
    public class TournamentTest
    {
        [Fact]
        public void TestGameStatePreFlop()
        {
            Player p1 = new Player(1, "TestPlayer1");
            Player p2 = new Player(2, "TestPlayer2");
            Tournament tournament = new Tournament();
            tournament.AddPlayer(p1);
            tournament.AddPlayer(p2);
            tournament.GameState = GameState.PreFlop;

            Assert.Equal(2, p1.StartingHand.Count);
            Assert.Equal(2, p2.StartingHand.Count);
            Assert.Equal(2, tournament.Players.Count);
        }

        [Fact]
        public void TestTournamentScenario1()
        {
            Player p1 = new Player(1, "TestPlayer1");
            Player p2 = new Player(2, "TestPlayer2");
            Tournament tournament = new Tournament();
            tournament.AddPlayer(p1);
            tournament.AddPlayer(p2);
            tournament.GameState = GameState.PreFlop;

            Assert.Equal(2, p1.StartingHand.Count);
            Assert.Equal(2, p2.StartingHand.Count);

            Player p = tournament.GetNextTurnPlayer();
            tournament.Bet(p.PlayerId, 800);
            p = tournament.GetNextTurnPlayer();
            tournament.Call(p.PlayerId);
            p = tournament.GetNextTurnPlayer();
            tournament.GameState = GameState.Flop;
            // TODO
        }
    }
}
