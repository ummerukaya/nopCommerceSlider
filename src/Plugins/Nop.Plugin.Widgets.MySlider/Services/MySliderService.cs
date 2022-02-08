using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IRepository<MySliderItem> _sliderItemRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<MySliders> _sliderRepository;
        private readonly IEventPublisher _eventPublisher;

        public MySliderService()
        {

        }
    }
}
