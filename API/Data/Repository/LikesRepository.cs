using API.Extensions;

namespace API.Data.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private readonly AppDbContext _context;

        public LikesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserLikes> GetUserLikes(int sourceUserId, int targetUserId)
        {

            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PageList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            var users= _context.AppUsers.OrderBy(s=>s.UserName).AsQueryable();
            var likes= _context.Likes.AsQueryable();
            if (likesParams.predicate == "liked")
            {
                likes = likes.Where(s => s.SourceUserId == likesParams.UserId);
                users = likes.Select(s => s.TargerUser);
            }
            if (likesParams.predicate == "likedBy")
            {
                likes = likes.Where(s => s.TargerUserId == likesParams.UserId);
                users = likes.Select(s => s.SourceUser);
            }
            var Likesuser = users.Select(u => new LikeDTO
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                PhotoUrl = u.Photos.FirstOrDefault(s => s.IsMain).Url,
                Age = u.DateOfBirth.CalculateAge(),
                Id = u.Id,
                Created = u.Created,
                LastActive = u.LastActive,




            });
            return await PageList<LikeDTO>.CreateAsync(Likesuser, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.AppUsers.Include(s=>s.LikedUsers).FirstOrDefaultAsync(s=>s.Id==userId);
        }
    }
}
