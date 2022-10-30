using FluentValidation;
using MovieAdvice.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Validations
{
    public class MovieRateModelValidation : AbstractValidator<MovieRateModel>, IBaseValidator
    {
        public MovieRateModelValidation()
        {
            RuleFor(x => x.Rate).NotNull().WithMessage("Puan giriniz.");

            RuleFor(x => x.MovieId).NotNull().WithMessage("Film seçiniz.");

            RuleFor(x => x).Must(CheckRate).WithMessage("Oy değeri 1-10 arasındaki değerleri alabilir.");
        }

        public bool CheckRate(MovieRateModel model)
        {
            if (model.Rate >= 1 && model.Rate <= 10)
            {
                return true;
            }
            return false;
        }
    }
}
