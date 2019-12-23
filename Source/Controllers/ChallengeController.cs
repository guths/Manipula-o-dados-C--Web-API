using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Codenation.Challenge.DTOs;
using Codenation.Challenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace Codenation.Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChallengeService _challengeService;
        public ChallengeController(IChallengeService service, IMapper mapper)
        {
            _mapper = mapper;
            _challengeService = service;
        }

        [HttpGet]
        [Route("api/challenge")]
        public ActionResult<IEnumerable<ChallengeDTO>> GetAll(int? accelerationId = null, int? userId = null)
        {
            if ((accelerationId == null && userId == null))
            {
                return NoContent();
            }
            else
            {
                var userIdNt = userId.GetValueOrDefault();
                var accelerationIdNt = accelerationId.GetValueOrDefault();
                var challenges = _challengeService.FindByAccelerationIdAndUserId(userIdNt, accelerationIdNt).AsQueryable();
                return Ok(_mapper.ProjectTo<ChallengeDTO>(challenges).ToList());
            }
        }
        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody] ChallengeDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }
            try
            {
                var challenge = _challengeService.Save(_mapper.Map<Models.Challenge>(value));
                return Ok(_mapper.Map<ChallengeDTO>(challenge));
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
