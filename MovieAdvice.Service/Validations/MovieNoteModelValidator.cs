using FluentValidation;
using MovieAdvice.Storage.Models;

namespace MovieAdvice.Service.Validations
{
    public class MovieNoteModelValidator : AbstractValidator<MovieNoteModel>, IBaseValidator
    {
        public MovieNoteModelValidator()
        {
            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Not giriniz.")
                .NotNull().WithMessage("Not giriniz.");

            RuleFor(x => x.MovieId).NotNull().WithMessage("Film seçiniz.");
        }
    }
}
