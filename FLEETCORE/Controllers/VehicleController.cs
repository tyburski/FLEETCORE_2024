using FLEETCORE.Models;
using FLEETCORE.Models.Body;
using FLEETCORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLEETCORE.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [AllowAnonymous]
        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    string[] brands = { "Citroën", "Fiat", "Ford", "Iveco", "MAN", "Mercedes-Benz", "DAF", "Volvo", "Renault", "Scania", "Peugeot" };
                    return Ok(brands);
                }
                else return Unauthorized();
            }
            else return Unauthorized();
            
        }
        [HttpPost("create")]
        public IActionResult Create(CreateVehicleBody body)
        {
            var currentUser = HttpContext.User;
            var result = "no_access";
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    result = vehicleService.Create(body);
                }
            }

            if (result.Equals("done")) return Ok(result);
            else return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var currentUser = HttpContext.User;
            var result = "no_access";
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    result = vehicleService.Delete(id);
                }
            }

            if (result.Equals("done")) return Ok(result);
            else return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll() 
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    return Ok(vehicleService.GetAll());
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        [HttpGet("get")]
        public IActionResult Get(int id) 
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    var result = vehicleService.Get(id);
                    if (result is null) return BadRequest("not_exist");
                    else return Ok(result);
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        [HttpPost("settag")]
        public IActionResult SetTag(int tagId, int vehicleId)
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    var result = vehicleService.SetTag(tagId, vehicleId);
                    if (!result.Equals("done")) return BadRequest(result);
                    else return Ok(result);
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        [HttpPost("removetag")]
        public IActionResult RemoveTag(int id)
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    var result = vehicleService.RemoveTag(id);
                    if (!result.Equals("done")) return BadRequest(result);
                    else return Ok(result);
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }
}
