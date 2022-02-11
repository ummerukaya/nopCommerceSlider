using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Models;

namespace Nop.Plugin.Widgets.MySlider.Factories
{
    public interface IMySliderModelFactory
    {
        Task<IList<SliderModel>> PrepareSliderListModelAsync(List<MySliders> sliders);
    }
}
