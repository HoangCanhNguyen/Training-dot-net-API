using FluentValidation;
using HotelListing.Models;

namespace HotelListing.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            Include(new UserLoginValidator());
            RuleFor(user => user.PhoneNumber).Matches(@"^(\+[0-9]{9})$").WithMessage("Phone number is less than 9 characters");
            RuleFor(user => user.Roles)
                .NotNull().WithMessage("Roles is required !")
                .NotEmpty().WithMessage("Roles is required 1");
            RuleForEach(user => user.Roles)
                .NotNull().WithMessage("Roles is required 2")
                .NotEmpty().WithMessage("Roles is required 3");
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
