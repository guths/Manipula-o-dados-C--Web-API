using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Codenation.Challenge.DTOs;
using Codenation.Challenge.Models;
using Codenation.Challenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace Codenation.Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubmissionService _submissionService;
        public SubmissionController(ISubmissionService service, IMapper mapper)
        {
            _mapper = mapper;
            _submissionService = service;
        }

        //Get api/submission
        [HttpGet]
        public ActionResult<IEnumerable<SubmissionDTO>> GetAll(int? challengeId = null, int? accelerationId = null)
        {
            if ((challengeId==null && accelerationId == null))
            {
                return NoContent();
            }
            else
            {
                var challengeIdNt = challengeId.GetValueOrDefault();
                var accelerationIdNt = accelerationId.GetValueOrDefault();

                var submissions = _submissionService.FindByChallengeIdAndAccelerationId(challengeIdNt,accelerationIdNt);
                return Ok(_mapper.Map<List<SubmissionDTO>>(submissions));
   
            }

            throw new Exception();
        }

        [HttpGet("{higherScore}")]
        public ActionResult<decimal> GetHigherScore(int? challengeId)
        {
            if (challengeId == null)
            {
                return NoContent();
            }
            else
            {
                var challengeIdNt = challengeId.GetValueOrDefault();
                var highscore = _submissionService.FindHigherScoreByChallengeId(challengeIdNt);
                return Ok(highscore);
            }
        }

        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody] SubmissionDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }

            try
            {
                var submission = _submissionService.Save(_mapper.Map<Submission>(value));
                return Ok(_mapper.Map<SubmissionDTO>(submission));
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }



    }
}
