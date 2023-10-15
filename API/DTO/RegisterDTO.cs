
namespace API.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [StringLength(  8,MinimumLength =4, ErrorMessage = "Password must be at least 8 characters long ")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
        public string Password { get; set; }
    }
}
