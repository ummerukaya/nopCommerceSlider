using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Services
{
    public class MySliderService : IMySliderService
    {
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<MySliderItem> _mysliderItemRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<MySliders> _mysliderRepository;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;

        public MySliderService(IRepository<MySliders> mysliderRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IRepository<MySliderItem> mysliderItemRepository,
            IStaticCacheManager cacheManager,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher)
        {
            _mysliderRepository = mysliderRepository;
            _storeMappingRepository = storeMappingRepository;
            _mysliderItemRepository = mysliderItemRepository;
            _catalogSettings = catalogSettings;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
        }


        //slider
        public async Task<IPagedList<MySliders>> GetAllSlidersAsync(List<int> widgetZoneIds = null, int storeId = 0, bool? active = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var sliders = _mysliderRepository.Table.Where(x => !x.Deleted);

            if (widgetZoneIds != null && widgetZoneIds.Any())
                sliders = sliders.Where(x => widgetZoneIds.Contains(x.WidgetZoneId));

            if (active.HasValue)
                sliders = sliders.Where(x => x.Active == active.Value);

            if (storeId > 0 && !_catalogSettings.IgnoreStoreLimitations)
            {
                sliders = from s in sliders
                          join sm in _storeMappingRepository.Table
                          on new { c1 = s.Id, c2 = nameof(MySliders) } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into slider_sm
                          from sm in slider_sm.DefaultIfEmpty()
                            where !s.LimitedToStores || storeId == sm.StoreId
                          select s;
            }

            var query = sliders.OrderBy(x => x.DisplayOrder);
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public async Task<MySliders> GetSliderByIdAsync(int sliderId)
        {
            if (sliderId == 0)
                return null;

            return await _mysliderRepository.GetByIdAsync(sliderId);
        }

        public async Task InsertSliderAsync(MySliders slider)
        {
            if (slider == null)
                throw new ArgumentNullException(nameof(slider));

            await _mysliderRepository.InsertAsync(slider);

            await _eventPublisher.EntityInsertedAsync(slider);
        }

        public async Task DeleteSliderAsync(MySliders slider)
        {
            if (slider == null)
                throw new ArgumentNullException(nameof(slider));

            await _mysliderRepository.DeleteAsync(slider);

            await _eventPublisher.EntityDeletedAsync(slider);
        }

        public async Task UpdateSliderAsync(MySliders slider)
        {
            if (slider == null)
                throw new ArgumentNullException(nameof(slider));

            await _mysliderRepository.UpdateAsync(slider);

            await _eventPublisher.EntityUpdatedAsync(slider);
        }


        //slider items
        public async Task<IPagedList<MySliderItem>> GetSliderItemsBySliderIdAsync(int sliderId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _mysliderItemRepository.Table;

            query = query.Where(sliderItem => sliderItem.SliderId == sliderId)
                .OrderBy(sliderItem => sliderItem.DisplayOrder);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public async Task<MySliderItem> GetSliderItemByIdAsync(int sliderItemId)
        {
            if (sliderItemId == 0)
                return null;

            return await _mysliderItemRepository.GetByIdAsync(sliderItemId);
        }

        public async Task InsertSliderItemAsync(MySliderItem sliderItem)
        {
            if (sliderItem == null)
                throw new ArgumentNullException(nameof(sliderItem));

            await _mysliderItemRepository.InsertAsync(sliderItem);

            await _eventPublisher.EntityInsertedAsync(sliderItem);
        }
   
        public async Task UpdateSliderItemAsync(MySliderItem sliderItem)
        {
            if (sliderItem == null)
                throw new ArgumentNullException(nameof(sliderItem));

            await _mysliderItemRepository.UpdateAsync(sliderItem);

            await _eventPublisher.EntityUpdatedAsync(sliderItem);
        }

        public async Task DeleteSliderItemAsync(MySliderItem sliderItem)
        {
            if (sliderItem == null)
                throw new ArgumentNullException(nameof(sliderItem));

            await _mysliderItemRepository.DeleteAsync(sliderItem);

            await _eventPublisher.EntityDeletedAsync(sliderItem);
        }
    }
}
