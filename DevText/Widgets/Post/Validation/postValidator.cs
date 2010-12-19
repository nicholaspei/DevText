using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevText.Framework.Validation;
using FluentValidation;
using Post.Model;

namespace Post.Validation
{
    public class postValidator:AbstractValidator<post>
    {
        public postValidator()
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.Content).NotNull();
        }

    }
}