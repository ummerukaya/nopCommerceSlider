using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.MySlider
{
    public class MySliderSettings : ISettings
    {
        public bool EnableSlider { get; set; }
        public string SelectedCustomerRoleIds { get; set; }
        //public IList<SelectListItem> AvailableCustomerRoles { get; set; }
    }
}
