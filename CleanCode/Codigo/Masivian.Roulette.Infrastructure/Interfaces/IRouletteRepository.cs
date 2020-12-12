using Masivian.Roulette.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Masivian.Roulette.Infrastructure.Interfaces
{
    public interface IRouletteRepository
    {
        Task<string> SaveRoulette(SpinWheel roulette);
        Task<List<SpinWheel>> GetAllRoulletes();
        Task<SpinWheel> getRoulette(string idRoulette);
        Task<OutcomingMessage> OpenRoulette(SpinWheel roulettee);
        Task<bool> SaveBet(Bet bet);
        Task<List<Bet>> getBetsRoulette(string idRoulette);
    }
}
