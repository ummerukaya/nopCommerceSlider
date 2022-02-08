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
    public record MySliderModel : BaseNopModel
    {
        public MySliderModel()
        {
            AvailableWidgetZones = new List<SelectListItem>();
            AvailableAnimationTypes = new List<SelectListItem>();
            SelectedStoreIds = new List<int>();
        }

        [NopResourceDisplayName("Nop.MySlider.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public int WidgetZoneId { get; set; }

        [NopResourceDisplayName("Nop.MySlider.Fields.WidgetZone")]
        public string WidgetZoneStr { get; set; }

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
        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableWidgetZones { get; set; }

        public IList<SelectListItem> AvailableAnimationTypes { get; set; }

       
    }
}
