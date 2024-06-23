using FLEETCORE.Models.Body;
using FLEETCORE.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FLEETCORE.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody]SignInBody body)
        {
            var result = accountService.SignIn(body);
            if (result.Equals("unauthorized") || result is null) return Unauthorized();
            else return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody]SignUpBody body)
        {
            var result = accountService.SignUp(body);
            var email = new EmailAddressAttribute();
            if (!email.IsValid(result)) return BadRequest(result);
            else return Ok(result);
        }
        
        [HttpPut("switchrole")]      
        public IActionResult SwitchRole(int id)
        {
            var currentUser = HttpContext.User;
            var result = "no_access";
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) == 2)
                {
                    result = accountService.SwitchRole(id);
                }
                else return BadRequest(result);
            }
            else return BadRequest(result);
             
            if (result.Equals("wrong_user")) return BadRequest(result);
            else return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var currentUser = HttpContext.User;
            var result = "no_access";
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) == 2)
                {
                    result = accountService.Delete(id, currentUser.Claims.FirstOrDefault(c => c.Type == "User").Value);
                    return Ok(result);
                }
                else return BadRequest(result);
            }
            else return BadRequest(result);
        }


    }
}