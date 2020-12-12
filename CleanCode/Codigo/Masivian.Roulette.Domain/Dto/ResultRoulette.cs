using Masivian.Roulette.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Masivian.Roulette.Domain.Dto
{
    public class ResultRoulette
    {
        public int WinningNumber { get; set; }
        public string Message { get; set; }
        public List<Winners> BetWinnersNumber { get; set; }
        public List<Winners> BetWinnersColor { get; set; }
    }
}
