using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Plugin.Widgets.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Factories
{
    public interface IMySliderAdminModelFactory
    {
        Task<ConfigurationModel> PrepareConfigurationModelAsync();

        Task<MySliderSearchModel> PrepareSliderSearchModelAsync(MySliderSearchModel sliderSearchModel);

        Task<MySliderListModel> PrepareSliderListModelAsync(MySliderSearchModel slidersearchModel);

        Task<MySliderModel> PrepareSliderModelAsync(MySliderModel model, MySliders slider,bool excludeProperties = false);

        Task<MySliderItemListModel> PrepareSliderItemListModelAsync(MySliderItemSearchModel searchModel);

        Task<MySliderItemModel> PrepareSliderItemModelAsync(MySliderItemModel model, MySliders slider,
            MySliderItem sliderItem, bool excludeProperties = false);
    }
}
