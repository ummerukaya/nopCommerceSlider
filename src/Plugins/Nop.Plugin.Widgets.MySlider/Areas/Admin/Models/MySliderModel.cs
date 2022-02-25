using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Models
{
    public record MySliderModel : BaseNopEntityModel,IStoreMappingSupportedModel
    {
        public MySliderModel()
        {
            AvailableWidgetZones = new List<SelectListItem>();
            AvailableCustomerRoles = new List<SelectListItem>();
            AvailableProductWidgetZones = new List<SelectListItem>();
            AvailableCategoryWidgetZones = new List<SelectListItem>();
            AvailableManufactureWidgetZones = new List<SelectListItem>();
            AvailableCatalogPages = new List<SelectListItem>();
            AvailableAnimationTypes = new List<SelectListItem>();
            SliderItemSearchModel = new MySliderItemSearchModel();
            SelectedStoreIds = new List<int>();
            SelectedCustomerRoleIds = new List<int>() { 0 };

        }

        [NopResourceDisplayName("Nop.MySlider.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public int CategoryWidgetZoneId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public int ProductWidgetZoneId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public int ManufactureWidgetZoneId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.OverrideGlobalSettings")]
        public bool OverrideGlobalSettings { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public string WidgetZoneStr { get; set; }

        public IList<string> CustomerRoleStr { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.CatalogPage")]
        public int CatalogPageId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.CustomerRole")]
        public IList<int> SelectedCustomerRoleIds { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.CatalogPage")]
        public string CatalogPageStr { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Nop.MySlider.Fields.BackGroundPicture")]
        public int BackGroundPictureId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.Nav")]
        public bool Nav { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.AutoPlay")]
        public bool AutoPlay { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.AutoPlayTimeout")]
        public int AutoPlayTimeout { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.AutoPlayHoverPause")]
        public bool AutoPlayHoverPause { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.StartPosition")]
        public int StartPosition { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.LazyLoad")]
        public bool LazyLoad { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.LazyLoadEager")]
        public int LazyLoadEager { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.Loop")]
        public bool Loop { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.Margin")]
        public int Margin { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.AnimateOut")]
        public string AnimateOut { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.AnimateIn")]
        public string AnimateIn { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.SelectedStoreIds")]
        public IList<int> SelectedStoreIds { get; set; }

        //[NopResourceDisplayName("Nop.MySlider.Fields.SelectedCustomerRoleIds")]
        //public IList<int> SelectedCustomerRoleIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }
        public IList<SelectListItem> AvailableWidgetZones { get; set; }
        public IList<SelectListItem> AvailableProductWidgetZones { get; set; }
        public IList<SelectListItem> AvailableCategoryWidgetZones { get; set; }
        public IList<SelectListItem> AvailableManufactureWidgetZones { get; set; }
        public IList<SelectListItem> AvailableCatalogPages { get; set; }

        public MySliderItemSearchModel SliderItemSearchModel { get; set; }

        public IList<SelectListItem> AvailableAnimationTypes { get; set; }

       
    }
}
