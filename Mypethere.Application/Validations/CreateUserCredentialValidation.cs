using FluentValidation;

using Mypethere.User.Application.DTOs.Request;

namespace Mypethere.User.Application.Validations
{
    public class CreateUserCredentialValidation : AbstractValidator<CreateUserLoginRequestDto>
    {
        public CreateUserCredentialValidation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.BirthDate)
                .NotNull()
                .Must(x => x == DateOnly.MinValue)
                .WithMessage("Name can't be null or empty");
            RuleFor(x => x.Password)
                .Equal(x => x.ReEnterPassword)
                .WithMessage("Password must be same than ReEnterPassword");
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ReEnterPassword).NotNull().NotEmpty();
        }
    }
}
