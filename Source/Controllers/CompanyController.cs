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
    public class CompanyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService service, IMapper mapper)
        {
            _mapper = mapper;
            _companyService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyDTO>> GetAll(int? accelerationId = null, int? userId = null)
        {
            if ((accelerationId == null && userId == null) || (accelerationId != null && userId != null))
            {
                return NoContent();
            }
            else
            {
                if (accelerationId != null)
                {
                    var accelerationIdNt = accelerationId.GetValueOrDefault();
                    var companies = _companyService.FindByAccelerationId(accelerationIdNt).AsQueryable();
                    return Ok(_mapper.Map<List<CompanyDTO>>(companies).ToList());
                }
                if (userId != null)
                {
                    var userIdNt = userId.GetValueOrDefault();
                    var companies = _companyService.FindByUserId(userIdNt).AsQueryable();
                    return Ok(_mapper.Map<List<CompanyDTO>>(companies).ToList());
                }
            }

            throw new Exception();
        }

        [HttpGet("{id}")]
        public ActionResult<CompanyDTO> Get (int? id)
        {
            if(id == null)
            {
                return NoContent();
            }
            var Id = id.GetValueOrDefault();
            try
            {
                var company = _companyService.FindById(Id);
                return Ok(_mapper.Map<CompanyDTO>(company));
            }
            catch (Exception)
            {

                return BadRequest(new { Menssagem = "Ocorreu um erro ao buscar o usuario" });
            }
            
        }
        [HttpPost]
        public ActionResult<CompanyDTO> Post([FromBody] CompanyDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }
            try
            {
                var company = _companyService.Save(_mapper.Map<Company>(value));
                return Ok(_mapper.Map<CompanyDTO>(company));
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }
    }



}
