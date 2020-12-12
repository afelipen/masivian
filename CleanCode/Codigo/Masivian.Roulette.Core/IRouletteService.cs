using Masivian.Roulette.Domain.Dto;
using Masivian.Roulette.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Masivian.Roulette.Core
{
    public interface IRouletteService
    {
        Task<string> Create();
        Task<string> OpenRoulette(string id);
        Task<Bet> Bet(string idRoulette, BetDto bet, string user);
        Task<ResultRoulette> CloseRoulette(string id);
        Task<IEnumerable<SpinWheel>> GetAllRoulletes();

    }
}
