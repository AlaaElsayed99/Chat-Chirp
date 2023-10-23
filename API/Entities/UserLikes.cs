namespace API.Entities
{
    public class UserLikes
    {
        public AppUser? SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser? TargerUser { get; set; }

        public int TargerUserId { get; set; }


    }
}
