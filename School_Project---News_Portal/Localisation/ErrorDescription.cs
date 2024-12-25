using Microsoft.AspNetCore.Identity;

namespace School_Project___News_Portal.Localisation
{
    public class ErrorDescription : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"{userName} already exists!" };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $"{email} already exists!" };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new() { Code = "PasswordRequiresNonAlphanumeric", Description = "Password requires at least one special character!" };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new() { Code = "PasswordRequiresDigit", Description = "Password requires at least one number (0-9) character!" };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = $"Password length needs to be at least {length}!" };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new() { Code = "PasswordRequiresLower", Description = $"Password requires at least one non-capital character!" };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new() { Code = "PasswordRequiresUpper", Description = $"Password requires at least one capital character!" };
        }
    }
}
