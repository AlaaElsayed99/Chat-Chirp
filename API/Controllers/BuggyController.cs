using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BuggyController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret Text";
        }
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var user = _context.AppUsers.Find(-1);
            if(user == null)
                return NotFound("Not Found");
            return Ok(user);
        }
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
           
                var user = _context.AppUsers.Find(-1);
                var returnUser = user.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, returnUser);
           

        }
        [HttpGet("Bad-Request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("There are a Bad Request");
            
        }
        


    }
}
