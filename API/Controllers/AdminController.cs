//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminController : ControllerBase
//    {
//        private readonly UserManager<AppUser> _userManager;

//        public AdminController(UserManager<AppUser> userManager)
//        {
//            _userManager = userManager;
//        }
//        [Authorize(Policy= "RequireAdminRole")]
//        [HttpGet("users-with-roles")]
//        public async Task<ActionResult> GetUserForRole()
//        {
//            var users = await _userManager.Users.OrderBy(u => u.UserName)
//                .Select(u => new
//                {
//                    u.Id,
//                    Username = u.UserName,
//                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
//                }).ToListAsync();
//            return Ok(users);
//        }

//        [Authorize(Policy = "RequireAdminRole")]
//        [HttpPost("edit-roles/{username}")]
//        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
//        {
//            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select one Role");

//            var selectedRole = roles.Split(",").ToArray();

//            var user = await _userManager.FindByNameAsync(username);
//            if (user == null) return NotFound("User not Found");

//            var userRoles= await _userManager.GetRolesAsync(user);

//            var result= await _userManager.AddToRolesAsync(user,selectedRole.Except(userRoles));

//            if (!result.Succeeded) return BadRequest(result.Errors);

//            result= await _userManager.RemoveFromRolesAsync(user, selectedRole.Except(userRoles));
//            if(!result.Succeeded) return BadRequest("failed to remove");

//            return Ok(await _userManager.GetRolesAsync(user));


//        }
//        [Authorize(Policy = "ModeratePhotoRole")]
//        [HttpGet("photos-to-moderate")]
//        public IActionResult GetPhotoForModerator()
//        {
//            return Ok("Moderator or admin can see this");
//        }

//    }
//}
