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
    public record MySliderSearchModel : BaseSearchModel
    {
        public MySliderSearchModel()
        {
            AvailableWidgetZones = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableActiveOptions = new List<SelectListItem>();
            SearchWidgetZones = new List<int>() { 0 };
            SearchCatalogPages = new List<int>() { 0 };
            AvailableCatalogPages = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Nop.MySlider.List.SearchWidgetZones")]
        public IList<int> SearchWidgetZones { get; set; }

        [NopResourceDisplayName("Nop.MySlider.List.SearchCatalogPages")]
        public IList<int> SearchCatalogPages { get; set; }

        [NopResourceDisplayName("Nop.MySlider.List.SearchStore")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.List.SearchActive")]
        public int SearchActiveId { get; set; }

        public IList<SelectListItem> AvailableWidgetZones { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableActiveOptions { get; set; }
        public IList<SelectListItem> AvailableCatalogPages { get; set; }

    }
}
