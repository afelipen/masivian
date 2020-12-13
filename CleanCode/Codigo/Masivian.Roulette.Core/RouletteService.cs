using Masivian.Roulette.Core.Exceptions;
using Masivian.Roulette.Domain.Dto;
using Masivian.Roulette.Domain.Entities;
using Masivian.Roulette.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masivian.Roulette.Core
{
    public class RouletteService : IRouletteService
    {
        public readonly IRouletteRepository rouletteRepository;
        public RouletteService(IRouletteRepository rouletteRepository)
        {
            this.rouletteRepository = rouletteRepository;
        }
        public async Task<string> Create()
        {
            SpinWheel roulette = new SpinWheel()
            {
                Id = Guid.NewGuid().ToString(),
                isOpen = false,
                dateOpen = null,
                dateClose = null
            };
            string id = await this.rouletteRepository.SaveRoulette(roulette);
            return id;
        }
        public async Task<IEnumerable<SpinWheel>> GetAllRoulletes()
        {
            return await this.rouletteRepository.GetAllRoulletes();
        }
        public async Task<string> OpenRoulette(string idRoulette)
        {
            SpinWheel roullete = await this.rouletteRepository.getRoulette(idRoulette);
            if (roullete == null)
                throw new BusinessException("La ruleta que desea abrir no esta creada");

            if (roullete.isOpen)
                throw new BusinessException("La ruleta ya se encuentra abierta");

            roullete.isOpen = true;
            roullete.dateOpen = DateTime.UtcNow;
            OutcomingMessage response = await this.rouletteRepository.OpenRoulette(roullete);

            return response.message;
        }
        public async Task<ResultRoulette> CloseRoulette(string idRoulette)
        {
            SpinWheel roullete = await this.rouletteRepository.getRoulette(idRoulette);
            if (!roullete.isOpen)
                throw new BusinessException("La ruleta ya se encuentra cerrada");

            ResultRoulette result = await processBets(idRoulette);
            roullete.isOpen = false;
            roullete.dateClose = DateTime.UtcNow;
            bool response = await this.rouletteRepository.CloseRoulette(roullete);
            if (!response)
                throw new BusinessException("Error cerrando la ruleta, intente nuevamente");

            return result;
        }
        private async Task<ResultRoulette> processBets(string idRoulette)
        {
            List<Bet> bets = await this.rouletteRepository.getBetsRoulette(idRoulette);
            ResultRoulette result = handlerWinners(bets);
            return result;
        }
        private ResultRoulette handlerWinners(List<Bet> bets)
        {
            ResultRoulette result = new ResultRoulette();
            int numberWinner = generateRandomNumber();
            result.WinningNumber = numberWinner;
            var winnersNumber = bets.Where(x => x.TypeBet.Equals(TypeBet.Number) && x.Number.Equals(numberWinner)).ToList();
            var colorWinner = determineWinningColor(numberWinner);
            var winnerColor = bets.Where(x => x.TypeBet.Equals(TypeBet.Color) && x.BetColor.Equals(colorWinner)).ToList();
            if (winnersNumber.Any())
                result = calculateAmountNumber(winnersNumber, result);
            if (winnerColor.Any())
                result = calculateAmountColor(winnerColor, result);

            return result;
        }

        private ResultRoulette calculateAmountNumber(List<Bet> winners, ResultRoulette result)
        {
            List<Winners> winnersNumber = new List<Winners>();
            foreach (var item in winners)
            {
                var winner = new Winners
                {
                    User = item.UserBet,
                    Amount = item.Amount * 5,
                    TypeBet = item.TypeBet,
                };
                winnersNumber.Add(winner);
            }
            result.BetWinnersNumber = winnersNumber;
            return result;
        }
        private ResultRoulette calculateAmountColor(List<Bet> winners, ResultRoulette result)
        {
            List<Winners> winnersColor = new List<Winners>();
            foreach (var item in winners)
            {
                var winner = new Winners
                {
                    User = item.UserBet,
                    Amount = item.Amount * 1.8,
                    TypeBet = item.TypeBet
                };
                winnersColor.Add(winner);
            }
            result.BetWinnersColor = winnersColor;
            return result;
        }
        private ColorBet determineWinningColor(int number)
        {
            return (number % 2 == 0) ? ColorBet.Red : ColorBet.Black;
        }
        private int generateRandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 36);
        }
        public async Task<Bet> Bet(string idRoulette, BetDto bet, string user)
        {
            Bet betAmount = null;
            if (!await isOpenRoulette(idRoulette))
                throw new BusinessException("La ruleta no esta abierta");

            if (validateBet(bet) && validateAmount(bet))
            {
                betAmount = MappingDtoToBet(bet, user, idRoulette);
                await this.rouletteRepository.SaveBet(betAmount);

                return betAmount;
            }

            return betAmount;
        }
        private Bet MappingDtoToBet(BetDto bet, string user, string idRoulette)
        {
            Bet betAmount = new Bet();
            betAmount.UserBet = user;
            betAmount.Amount = bet.Amount;
            betAmount.TypeBet = bet.TypeBet;
            betAmount.Number = bet.Number;
            betAmount.BetColor = bet.BetColor;
            betAmount.DateBet = DateTime.UtcNow;
            betAmount.RouletteId = idRoulette;
            return betAmount;
        }
        private async Task<bool> isOpenRoulette(string idRoulette)
        {
            SpinWheel roullete = await this.rouletteRepository.getRoulette(idRoulette);
            if (!roullete.isOpen)
                return false;

            return true;
        }
        private bool validateBet(BetDto bet)
        {
            if (bet.TypeBet.Equals(TypeBet.Number))
            {
                if (bet.Number < 0 || bet.Number > 36)
                    throw new BusinessException("El número al que desea apostar no es valido - debe ser entre 0 al 36");
            }
            return true;
        }
        private bool validateAmount(BetDto bet)
        {
            if (bet.Amount > 10000 || bet.Amount < 1)
            {
                throw new BusinessException("El monto apostado es superior a 10.000 dolares o menor a un 1 dolar");
            }

            return true;
        }
    }
}
