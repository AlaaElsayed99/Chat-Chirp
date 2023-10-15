namespace API.CustomValidation
{
    public class UniqueUsernameAtrribute: ValidationAttribute
    {
        private readonly AppDbContext _context;

        public UniqueUsernameAtrribute(AppDbContext context)
        {
            _context = context;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {


            string Username = value.ToString();

            AppUser UserDb = _context.AppUsers.FirstOrDefault(s => s.UserName == Username);
            AppUser UserRQ = validationContext.ObjectInstance as AppUser;

            if (UserDb == null) // في حالة ال  add 
            {
                return ValidationResult.Success;
            }
            else if (UserRQ.Id == UserDb.Id) // في حالة edit
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("name aleardy found");

            //
            //return base.IsValid(value, validationContext);
        }
    }
}
