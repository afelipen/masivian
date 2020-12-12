using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Masivian.Roulette.Core;
using Masivian.Roulette.Domain.Dto;
using Masivian.Roulette.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masivian.Roulette.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService rouletteService;
        public RouletteController(IRouletteService rouletteService)
        {
            this.rouletteService = rouletteService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("createRoulette")]
        public async Task<IActionResult> CreateRoulette()
        {
            string spinWheelId = await rouletteService.Create();
            return Ok(spinWheelId);
        }

        [HttpPut("openRoulette/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> OpenRoulette(string Id)
        {
            string message = await rouletteService.OpenRoulette(Id);
            return Ok(message);
        }

        [HttpPut("closeRoulette/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CloseRoulette(string Id)
        {
            var response = await rouletteService.CloseRoulette(Id);
            return Ok(response);
        }

        [HttpPost("Bet/{roulette}")]
        public async Task<IActionResult> Bet([FromHeader(Name = "userId")] string userId, string roulette, [FromBody] BetDto data)
        {
            Bet response = await rouletteService.Bet(roulette, data, userId);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("getAllRoulette")]
        public async Task<IActionResult> GetAllRoulletes()
        {
            var response = await rouletteService.GetAllRoulletes();
            return Ok(response);
        }
    }
}