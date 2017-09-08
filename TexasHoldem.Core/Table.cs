using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class Table
    {
        public decimal Stake { get; set; }
        public List<Pot> Pots;
        private Tournament game;
	
	    private Pot GetNextPotAfter(Pot pot)
        {
            int potIndex = Pots.IndexOf(pot);
            if (potIndex < 0)
            {
                // TODO: throw Exception
            }
            if (Pots.Count < potIndex + 2)
            {
                Pots.Add(new Pot());
            }
            return Pots[potIndex + 1];
        }

        public Table(Tournament game)
        {
            this.game = game;
            this.Stake = game.Settings.Stake;

            // Initializing the Main Pot and Side-Pots
            // Main Pot is the first pot in the array
            this.Pots = new List<Pot>(8);
            for (int i = 0; i < 8; i++)
            {
                this.Pots.Add(new Pot());
            }
        }

        public void ClearPots()
        {
            Pots.ForEach(p => p.Clear());
        }

        public void Contribute(Player contributor, decimal chips)
        {
            decimal remainingChips = 0;

            if (contributor.Chips <= chips)
            {
                // All-In, baby
                remainingChips = contributor.Chips;
            }
            else
            {
                remainingChips = chips;
            }

            Pot pot = null;

            while (remainingChips > 0)
            {
                if (pot == null)
                {
                    pot = Pots[0];
                }
                else
                {
                    pot = GetNextPotAfter(pot);
                }

                Dictionary<Player, decimal> potContributions = pot.Contributions;

                decimal contributorSum = 0;
                if (potContributions.ContainsKey(contributor))
                {
                    contributorSum = potContributions[contributor];
                }

                decimal potCap = pot.Cap;
                // If there is a cap in this pot, contribute the difference to the pot
                if (potCap > 0)
                {
                    if (contributorSum < potCap)
                    {
                        decimal potSumLeft = potCap - contributorSum;
                        remainingChips -= potSumLeft;
                        contributor.Chips -= potSumLeft;
                        pot.AddChips(contributor, potSumLeft);
                    }
                    else
                    {
                        // This pot is full, move to the next one
                        continue;
                    }
                }
                else
                {
                    // This pot is unlimited, we are free to contribute the remaining chips					
                    // However, it's essential that we check other players' contributions
                    // It may happen that we need to create another pot if someone has 
                    // already contributed more than current player has.
                    decimal contributorFutureSum = contributorSum + remainingChips;

                    Pot newSidePot = GetNextPotAfter(pot);

                    bool potGetsCapped = false;
                    // The sums in the current pot get equalized if there are values
                    // higher than contributed chips. 
                    // The difference will be transfered to the new pot
                    foreach (Player player in game.Players)
                    {
                        if (player != contributor && potContributions.ContainsKey(player))
                        {
                            decimal otherPlayerContributedSum = potContributions[player];
                            if (otherPlayerContributedSum > contributorFutureSum)
                            {
                                potGetsCapped = true;
                                decimal difference = otherPlayerContributedSum - contributorFutureSum;
                                pot.RemoveChips(player, difference);
                                newSidePot.AddChips(player, difference);
                            }
                        }
                    }
                    if (potGetsCapped)
                    {
                        pot.Cap = contributorFutureSum;
                    }
                    pot.AddChips(contributor, remainingChips);
                    contributor.Chips -= remainingChips;
                }
            }

        }

    }
}
