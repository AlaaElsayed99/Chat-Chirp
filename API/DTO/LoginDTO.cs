namespace API.DTO
{
    public class LoginDTO
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
