using FluentValidation;
using MovieAdvice.Storage.Models;

namespace MovieAdvice.Service.Validations
{
    public class MovieAdviceModelValidation: AbstractValidator<MovieAdviceModel>, IBaseValidator
    {
        public MovieAdviceModelValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email adresi giriniz.")
                                 .NotNull().WithMessage("Email adresi giriniz.");

            RuleFor(x => x.MovieId).NotNull().WithMessage("Film seçiniz.");
        }
    }
}
