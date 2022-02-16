using FluentValidation;
using HotelListing.Models;

namespace HotelListing.Validators
{
    public class HotelValidator : AbstractValidator<CreateHotelDTO>
    {
        public HotelValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Address).NotEmpty();
            RuleFor(m => m.Rating)
                .NotEmpty()
                .InclusiveBetween(1, 5);
        }
    }
}
