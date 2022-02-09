using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Validators
{
    public class MySlidersValidator : BaseNopValidator<MySliderModel>
    {
        public MySlidersValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nop.MySlider.Sliders.Fields.Name.Required");
        }
    }
}
