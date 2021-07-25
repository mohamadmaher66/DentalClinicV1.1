using AutoMapper;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Request;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserModule;

namespace DentalClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserDSL userDSL;

        public UserController(IMapper _mapper)
        {
            userDSL = new UserDSL(_mapper);
        }

        [HttpPost]
        [Route("GetAllUsers")]
        public IActionResult GetUsers([FromBody] RequestedData<UserDTO> requestedData)
        {
            requestedData.EntityList = userDSL.GetAll(requestedData.GridSettings);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetAllUserLite")]
        public IActionResult GetAllUserLite([FromBody] RequestedData<UserDTO> requestedData)
        {
            requestedData.EntityList = userDSL.GetAllLite();
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("GetUser")]
        public IActionResult GetUser([FromBody] RequestedData<UserDTO> requestedData)
        {
            requestedData.Entity = userDSL.GetById(requestedData.Entity.Id);
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromBody] RequestedData<UserDTO> requestedData)
        {
            userDSL.Add(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم اضافة المستخدم بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("EditUser")]
        public IActionResult EditUser([FromBody] RequestedData<UserDTO> requestedData)
        {
            userDSL.Update(requestedData.Entity, requestedData.UserId);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم تعديل المستخدم بنجاح" });
            return Ok(requestedData);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public IActionResult DeleteUser([FromBody] RequestedData<UserDTO> requestedData)
        {
            requestedData.EntityList = userDSL.Delete(requestedData.Entity, requestedData.GridSettings);
            requestedData.Alerts.Add(new Alert { Title = "تم بنجاح", Type = AlertTypeEnum.Success, Message = "تم حذف المستخدم بنجاح" });
            return Ok(requestedData);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] RequestedData<UserDTO> requestedData)
        {
            requestedData.Entity = userDSL.Login(requestedData.Entity.Username, requestedData.Entity.Password);
            if (requestedData.Entity != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("DentalClinicKey#12*"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] { new Claim(JwtRegisteredClaimNames.NameId, requestedData.Entity.Id.ToString()) };
                var token = new JwtSecurityToken(null, null, claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);

                requestedData.Entity.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }
            return Ok(requestedData);
        }
    }
}