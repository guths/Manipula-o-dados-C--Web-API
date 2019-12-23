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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserController(IUserService service, IMapper mapper)
        {
            _mapper = mapper;
            _userService = service;
        }

        //GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAll(string accelerationName = null, int? companyId = null)
        {
            if ((string.IsNullOrEmpty(accelerationName) && companyId==null) || (!string.IsNullOrEmpty(accelerationName) && companyId!=null))
            {
                return NoContent();
            }
            else
            {
                if (!string.IsNullOrEmpty(accelerationName))
                {
                    var users = _userService.FindByAccelerationName(accelerationName).AsQueryable();
                    return Ok(_mapper.ProjectTo<UserDTO>(users).ToList());
                }
                if (companyId != null)
                {
                    var companyIdNotNull = companyId.GetValueOrDefault();
                    var users = _userService.FindByCompanyId(companyIdNotNull).AsQueryable();
                    return Ok(_mapper.ProjectTo<UserDTO>(users).ToList());
                }
            }
            throw new Exception();
        }

        // GET api/user/{id}
        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get(int id)
        {
            try
            {
                var user = _userService.FindById(id);
                if (user == null)
                    return NoContent();
                else
                    return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception)
            {

               return BadRequest(new { Mensagem = "Ocorreu um erro ao buscar o usuario" });
                //return BadRequest(new BadRequestObjectResult("Erro ao trazer os dados do banco de dados."));
            }
        }

        //POST api/user
        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody] UserDTO value)
        {
            if (value == null)
            {
                return NoContent();
            }
            try
            {
                                
                var user = _userService.Save(_mapper.Map<User>(value));
                
                return Ok(_mapper.Map<UserDTO>(user));
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

    }
}
