using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Services
{
    public interface IMySliderService
    {

        Task<IPagedList<MySliders>> GetAllSlidersAsync(List<int> widgetZoneIds = null, int storeId = 0,
            bool? active = null, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<MySliders> GetSliderByIdAsync(int sliderId);

        Task InsertSliderAsync(MySliders slider);

        Task UpdateSliderAsync(MySliders slider);

        Task DeleteSliderAsync(MySliders slider);


        Task<IPagedList<MySliderItem>> GetSliderItemsBySliderIdAsync(int sliderId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<MySliderItem> GetSliderItemByIdAsync(int sliderItemId);

        Task InsertSliderItemAsync(MySliderItem sliderItem);

        Task UpdateSliderItemAsync(MySliderItem sliderItem);

        Task DeleteSliderItemAsync(MySliderItem sliderItem);

    }
}
