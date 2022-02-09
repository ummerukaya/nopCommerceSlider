using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.MySlider
{
    public class MySliderSettings : ISettings
    {
        public bool EnableSlider { get; set; }
    }
}
