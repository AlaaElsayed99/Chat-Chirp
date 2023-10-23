namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLikes> GetUserLikes(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PageList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}
