using FLEETCORE.Models.Body;
using FLEETCORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLEETCORE.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/tag")]

    public class TagController: ControllerBase
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpPost("create")]
        public IActionResult Create(string unique)
        {
            var currentUser = HttpContext.User;
            var result = "no_access";
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                if (Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value) > 0)
                {
                    result = tagService.Create(unique);
                }
            }

            if(result.Equals("done"))
            {
                return Ok(result);
            }
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
                    result = tagService.Delete(id);
                }
            }
            if(result.Equals("done")) return Ok(result);
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
                    return Ok(tagService.GetAll());
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
                    var result = tagService.Get(id);
                    if(result is null) return BadRequest("not_exist");
                    else return Ok(tagService.Get(id));
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }   
}
