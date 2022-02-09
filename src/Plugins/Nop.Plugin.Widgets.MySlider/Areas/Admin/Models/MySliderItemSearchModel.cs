using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Models
{
    public record MySliderItemSearchModel : BaseSearchModel
    {
        public int SliderId { get; set; }
    }
}
