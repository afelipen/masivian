using System;
using System.Collections.Generic;
using System.Text;

namespace Masivian.Roulette.Domain.Entities
{
    public class Winners
    {
        public string User { get; set; }
        public double Amount { get; set; }
        public TypeBet TypeBet { get; set; }
    }
}
