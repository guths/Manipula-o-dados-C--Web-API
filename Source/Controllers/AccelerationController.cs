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
    public class AccelerationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccelerationService _accelerationService;
        public AccelerationController(IAccelerationService service, IMapper mapper)
        {
            _mapper = mapper;
            _accelerationService = service;
        }

        public ActionResult<IEnumerable<AccelerationDTO>> GetAll(int? companyId = null)
        {
            if (companyId==null)
            {
                return NoContent();
            }
            else
            {
                var companyIdNt = companyId.GetValueOrDefault();
                var accelerations = _accelerationService.FindByCompanyId(companyIdNt).AsQueryable();
                return Ok(_mapper.Map<List<AccelerationDTO>>(accelerations).ToList());
            }

            throw new Exception();
        }

        [HttpGet("{id}")]
        public ActionResult<AccelerationDTO> Get(int? id)
        {
            if (id == null)
            {
                return NoContent();
            } 
            else
            {
                var idNotNull = id.GetValueOrDefault();
                var acceleration = _accelerationService.FindById(idNotNull);
                return Ok(_mapper.Map<AccelerationDTO>(acceleration));
            }
        }

        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody] AccelerationDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }

            try
            {
                var acceleration = _accelerationService.Save(_mapper.Map<Acceleration>(value));
                return Ok(_mapper.Map<AccelerationDTO>(acceleration));
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }



    }
}
