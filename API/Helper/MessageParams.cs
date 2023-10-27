namespace API.Helper
{
    public class MessageParams:Paginationparams
    {
        public string? Username { get; set; }
        public string Container { get; set; } = "Unread";

    }
}
