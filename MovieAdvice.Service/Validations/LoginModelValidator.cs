using FluentValidation;
using MovieAdvice.Storage.Models;

namespace MovieAdvice.Service.Validations
{
    public class LoginModelValidator: AbstractValidator<LoginModel>, IBaseValidator
    {
        public LoginModelValidator()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage("Email adresi giriniz.")
                                 .NotNull().WithMessage("Email adresi giriniz.");

            RuleFor(c => c.Password).NotEmpty().WithMessage("Şifre giriniz.")
                                    .NotNull().WithMessage("Şifre giriniz.");
        }
    }
}
