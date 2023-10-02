

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            var Users = await _context.appUsers.ToListAsync();
            return Ok(Users);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var User = await _context.appUsers.FindAsync(id);
            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }
    }
}
