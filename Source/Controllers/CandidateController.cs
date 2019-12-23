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
    public class CandidateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICandidateService _candidateService;
        public CandidateController(ICandidateService service, IMapper mapper)
        {
            _mapper = mapper;
            _candidateService = service;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<CandidateDTO>> GetAll(int? companyId = null, int? accelerationId = null)
        {
            if ((companyId == null && accelerationId == null) || (companyId != null && accelerationId != null))
            {
                return NoContent();
            }
            else
            {
                if (companyId != null)
                {
                    var companyIdNt = companyId.GetValueOrDefault();
                    var candidates = _candidateService.FindByCompanyId(companyIdNt).AsQueryable();
                    return Ok(_mapper.Map<List<CandidateDTO>>(candidates).ToList());
                }
                if (accelerationId != null)
                {
                    var accelerationIdNt = accelerationId.GetValueOrDefault();
                    var companyIdNotNull = companyId.GetValueOrDefault();
                    var candidates = _candidateService.FindByAccelerationId(accelerationIdNt).AsQueryable();
                    return Ok(_mapper.Map<List<CandidateDTO>>(candidates).ToList());
                }
            }
            throw new Exception();
        }

        [HttpGet("{userId}/{accelerationId/{companyId}")]
        public ActionResult<CandidateDTO> Get(int? userId, int? accelerationId, int? companyId)
        {
            if(userId==null || accelerationId==null || companyId == null)
            {
                return NoContent();
            }
            else
            {
                var userIdNt = userId.GetValueOrDefault();
                var accelerationIdNt = accelerationId.GetValueOrDefault();
                var companyIdNt = companyId.GetValueOrDefault();
                var candidate = _candidateService.FindById(userIdNt,accelerationIdNt,companyIdNt);
                return Ok(_mapper.Map<CandidateDTO>(candidate));
            }
        }

        [HttpPost]
        public ActionResult<CandidateDTO> Post([FromBody] CandidateDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }
            try
            {
                var candidate = _candidateService.Save(_mapper.Map<Candidate>(value));
                return Ok(_mapper.Map<CandidateDTO>(candidate));
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }
    }
}
