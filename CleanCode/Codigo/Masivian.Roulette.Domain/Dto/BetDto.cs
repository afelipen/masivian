using Masivian.Roulette.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Masivian.Roulette.Domain.Dto
{
    public class BetDto
    {
        public TypeBet TypeBet { get; set; }
        public int? Number { get; set; }
        public double Amount { get; set; }
        public ColorBet? BetColor { get; set; }
    }
}
