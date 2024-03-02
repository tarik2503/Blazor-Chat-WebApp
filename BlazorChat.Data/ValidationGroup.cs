using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChat.Data
{
    public class ValidationGroup : AbstractValidator<Groups>
    {
        public ValidationGroup()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty().WithMessage("*");
            RuleFor(x=> x.Name).MinimumLength(3).WithMessage("Minimum length is 3");
            RuleFor(x => x.Name).MaximumLength(25).WithMessage("Maximum length is 25");
        }
    }
}
