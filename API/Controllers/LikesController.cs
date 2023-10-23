using API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    public class LikesController : ControllerBase
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;

        public LikesController(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }
        [HttpPost("{username}")]
        public async Task<ActionResult> Addlike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("you can't like yourself");
            var userLike = await _likesRepository.GetUserLikes(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("you liked this user");
            userLike = new UserLikes
            {
                SourceUserId = sourceUserId,
                TargerUserId = likedUser.Id,

            };
            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok("You have liked this user");
            return BadRequest("failed to like user");
        }
        [HttpGet]
        public async Task<ActionResult<PageList<LikeDTO>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId=User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    } 
}    

            


    

