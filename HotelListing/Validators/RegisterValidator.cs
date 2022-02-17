using FluentValidation;
using HotelListing.Models;

namespace HotelListing.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        private readonly int _minLengthPassword = 6;
        private readonly int _maxLengthPassword = 50;
        public RegisterValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please ensure you have entered your {PropertyName}");

            RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull().WithMessage("Password is required").WithErrorCode("NotNullValidator")
                .Length(_minLengthPassword, _maxLengthPassword).WithMessage(
                    string.Format(
                        "Password must be to {0} to {1} characters",
                        _minLengthPassword,
                        _maxLengthPassword
                        )
                    )
                .Matches(@"[0-9]+").WithMessage("Please include at least one numberic character").WithErrorCode("ERR1")
                .Matches(@"[A-Z]+").WithMessage("Please include at least one uppercase character").WithErrorCode("ERR2")
                .Matches(@"[\p{P}\p{S}]").WithMessage("Please include at least one special character").WithErrorCode("ERR3")
                ;

            RuleFor(user => user.PhoneNumber).Matches(@"^(\+[0-9]{9})$").WithMessage("Phone number is less than 9 characters");

            RuleFor(user => user.Roles)
                .NotNull().WithMessage("Roles is required 1")
                .NotEmpty().WithMessage("Roles is required 2");

            RuleForEach(user => user.Roles)
                .NotNull().WithMessage("Roles is required 3")
                .NotEmpty().WithMessage("Roles is required 4");
        }
    }

    public class UserLoginValidator: AbstractValidator<LoginDTO>
    {
        private readonly int _minLengthPassword = 6;
        private readonly int _maxLengthPassword = 50;
        public UserLoginValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please ensure you have entered your {PropertyName}");

            RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull().WithMessage("Email is required")
                .Length(_minLengthPassword, _maxLengthPassword).WithMessage(
                    string.Format(
                        "Password must be to {0} to {1} characters",
                        _minLengthPassword,
                        _maxLengthPassword
                        )
                    );
        }
    }
}
