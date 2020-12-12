using Masivian.Roulette.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Masivian.Roulette.Domain.Entities
{
    public class Bet
    {
        public double Amount { get; set; }
        public TypeBet TypeBet { get; set; }
        public int? Number { get; set; }
        public ColorBet? BetColor { get; set; }
        public string UserBet { get; set; }
        public DateTime DateBet { get; set; }
        public string RouletteId { get; set; }

    }
}
