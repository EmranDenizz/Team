using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team.Business.Repository;
using Team.Business.Request;

namespace Team.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public TeamController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CreatePlayerRequest>> GetAllPlayer()
        {
            return await _playerRepository.GetAllPlayers();
        }
    }
}
