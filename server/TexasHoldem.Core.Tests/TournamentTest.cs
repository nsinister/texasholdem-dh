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

            Tournament game = new Tournament();

            game.AddPlayer(p1);
            game.AddPlayer(p2);
            game.AddPlayer(p3);

            Assert.Equal(3, game.Players.Count);

            // Pre-Flop
            game.NextRound();
            Assert.Equal(GameState.PreFlop, game.GameState);

            Assert.True(p1.IsDealer);
            Assert.False(p1.IsSmallBlind);
            Assert.False(p1.IsBigBlind);

            Assert.False(p2.IsDealer);
            Assert.True(p2.IsSmallBlind);
            Assert.False(p2.IsBigBlind);

            Assert.False(p3.IsDealer);
            Assert.False(p3.IsSmallBlind);
            Assert.True(p3.IsBigBlind);

            Player nextTurnPlayer = null;

            nextTurnPlayer = game.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(p2, nextTurnPlayer);

            Assert.NotNull(game.CurrentTurnPlayer);
            Assert.Equal(p1, game.CurrentTurnPlayer);
            bool playerTurnRes = game.Call(game.CurrentTurnPlayer);
            Assert.True(playerTurnRes);

            Assert.Equal(GameState.PreFlop, game.GameState);

            nextTurnPlayer = game.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(p3, nextTurnPlayer);

            Assert.NotNull(game.CurrentTurnPlayer);
            Assert.Equal(p2, game.CurrentTurnPlayer);
            playerTurnRes = game.Call(game.CurrentTurnPlayer);
            Assert.True(playerTurnRes);

            Assert.Equal(GameState.PreFlop, game.GameState);

            nextTurnPlayer = game.GetNextTurnPlayer();
            Assert.NotNull(nextTurnPlayer);
            Assert.Equal(p1, nextTurnPlayer);

            Assert.NotNull(game.CurrentTurnPlayer);
            Assert.Equal(p3, game.CurrentTurnPlayer);
            playerTurnRes = game.Check(game.CurrentTurnPlayer);
            Assert.True(playerTurnRes);

            // Last player action for this turn should have changed the game state to Flop
            Assert.Equal(GameState.Flop, game.GameState);

            var sharedCards = game.SharedCards;
            Assert.Equal(3, sharedCards.Length);
            
            // Players do a Check
            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);

            sharedCards = game.SharedCards;
            Assert.Equal(4, sharedCards.Length);

            // Players do a Check once again
            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);

            sharedCards = game.SharedCards;
            Assert.Equal(5, sharedCards.Length);

            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);
            game.Check(game.CurrentTurnPlayer);

            // there was a Showdown, so next hand should have been dealt by now
            Assert.Equal(GameState.PreFlop, game.GameState);
        }
    }
}
