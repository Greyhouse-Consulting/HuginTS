using System;
using FluentValidation;
using HuginTS.Service.Models;

namespace HuginTS.Service.Validators
{
    public class DataPointValidator : AbstractValidator<Datapoint>
    {
        public DataPointValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");

            RuleFor(x => x.Timestamp).NotNull().WithMessage("Timestamp cannot be null");

            RuleFor(x => x.Timestamp).GreaterThan(new DateTime(2000, 1, 1)).WithMessage("Timestamp has to be greater that 2000-01-01");

        }
    }
}