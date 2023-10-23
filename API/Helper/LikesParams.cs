namespace API.Helper
{
    public class LikesParams:Paginationparams
    {
        public  int UserId { get; set; }
        public string? predicate { get; set; }
    }
}
