using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class Pot
    {
        public decimal TotalChips { get; set; }
        public Dictionary<Player, decimal> Contributions { get; set; }
        public decimal Cap { get; set; }
        public HashSet<Player> Winners { get; set; }

        public Pot()
        {
            this.Contributions = new Dictionary<Player, decimal>();
            this.Winners = new HashSet<Player>();
        }

        public void AddChips(Player contributedPlayer, decimal chips)
        {
            decimal currentSum = 0;
            if (Contributions.ContainsKey(contributedPlayer))
            {
                currentSum = Contributions[contributedPlayer];
            }
            currentSum += chips;
            TotalChips += chips;
            Contributions.Add(contributedPlayer, currentSum);
        }

        public void RemoveChips(Player player, decimal chips)
        {
            if (!Contributions.ContainsKey(player))
            {
                return;
            }
            decimal currentSum = Contributions[player];
            if (chips <= currentSum)
            {
                currentSum -= chips;
                TotalChips -= chips;
                Contributions.Add(player, currentSum);
            }
            else
            {
                // TODO: throw Exception, maybe?
                TotalChips -= currentSum;
                Contributions.Add(player, 0);
            }

        }

        public void AddWinner(Player player)
        {
            this.Winners.Add(player);
        }

        public void Clear()
        {
            Contributions.Clear();
            Winners.Clear();
            TotalChips = 0;
            Cap = 0;
        }
    }
}
