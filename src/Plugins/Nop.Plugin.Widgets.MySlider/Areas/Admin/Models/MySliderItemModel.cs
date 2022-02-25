using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Models
{
    public record MySliderItemModel : BaseNopEntityModel
    {
        public MySliderItemModel()
        {
            
        }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.Title")]
        public string SliderItemTitle { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Nop.MySliderItems.Fields.Picture")]
        public int PictureId { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Nop.MySliderItems.Fields.MobilePicture")]
        public int MobilePictureId { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.ImageAltText")]
        public string ImageAltText { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.Link")]
        public string Link { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.Picture")]
        public string PictureUrl { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.MobilePicture")]
        public string MobilePictureUrl { get; set; }

        public string FullPictureUrl { get; set; }

        public string MobileFullPictureUrl { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public int MySlidersId { get; set; }

        [NopResourceDisplayName("Nop.MySliderItems.Fields.ShopNowLink")]
        public string ShopNowLink { get; set; }

    }

}
