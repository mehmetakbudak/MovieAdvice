using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Storage.Consts;
using System.Linq;

namespace MovieAdvice.Service.Validations
{
    public class ValidationHandling : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid && result.Errors != null && result.Errors.Any())
            {
                var notFoundError = result.Errors
                    .FirstOrDefault(x => x.ErrorCode == ErrorCodes.NotFound);

                if (notFoundError != null)
                {
                    throw new NotFoundException(notFoundError.ErrorMessage);
                }

                var error = result.Errors.Select(x => x.ErrorMessage).FirstOrDefault();

                throw new NotAcceptableException(error);
            }

            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
