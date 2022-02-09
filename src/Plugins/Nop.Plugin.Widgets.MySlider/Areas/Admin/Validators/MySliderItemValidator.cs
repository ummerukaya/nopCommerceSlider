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
    public class MySliderItemValidator : BaseNopValidator<MySliderItemModel>
    {
        public MySliderItemValidator()
        {
            RuleFor(x => x.PictureId).GreaterThan(0).WithMessage("Nop.MySlider.SliderItems.Fields.Picture.Required");
            RuleFor(x => x.MobilePictureId).GreaterThan(0).WithMessage("Nop.MySlider.Sliders.Fields.MobilePicture.Required");
        }
        
    }
}
