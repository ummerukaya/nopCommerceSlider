using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Models
{
    public record ConfigurationModel : BaseNopModel, ISettingsModel
    {
        public int ActiveStoreScopeConfiguration { get ; set ; }

        [NopResourceDisplayName("Nop.MySlider.Configuration.Fields.EnableSlider")]
        public bool EnableSlider { get; set; }
        public bool EnableSlider_OverrideForStore { get; set; }
    }
}
