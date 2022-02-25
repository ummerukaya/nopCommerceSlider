using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Models
{
    public record ConfigurationModel : BaseNopModel, ISettingsModel
    {
        public ConfigurationModel()
        {
            SelectedCustomerRoleIds = new List<int>() { 0 };
        }
        public int ActiveStoreScopeConfiguration { get ; set ; }

        [NopResourceDisplayName("Nop.MySlider.Configuration.Fields.EnableSlider")]
        public bool EnableSlider { get; set; }
        public bool EnableSlider_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Configuration.Fields.SelectedCustomerRoleIds")]
        public IList<int> SelectedCustomerRoleIds { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }
        public bool SelectedCustomerRoleIds_OverrideForStore { get; set; }
    }
}
