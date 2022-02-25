using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Core.Domain.Configuration;
using Nop.Core.Events;
using Nop.Plugin.MySlider.Domains;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.MySlider.Infrastructure.Cache
{
    
    public partial class ModelCacheEventConsumer :
        IConsumer<EntityInsertedEvent<Setting>>,
        IConsumer<EntityUpdatedEvent<Setting>>,
        IConsumer<EntityDeletedEvent<Setting>>,
        IConsumer<EntityInsertedEvent<MySliders>>,
        IConsumer<EntityUpdatedEvent<MySliders>>,
        IConsumer<EntityDeletedEvent<MySliders>>,
        IConsumer<EntityInsertedEvent<MySliderItem>>,
        IConsumer<EntityUpdatedEvent<MySliderItem>>,
        IConsumer<EntityDeletedEvent<MySliderItem>>
    {
       
        public static CacheKey PICTURE_URL_MODEL_KEY = new CacheKey("Nop.plugins.widgets.myslider.pictureurl-{0}-{1}", PICTURE_URL_PATTERN_KEY);
        public const string PICTURE_URL_PATTERN_KEY = "Nop.plugins.widgets.myslider.picture";

        public static CacheKey MY_SLIDER_MODEL_KEY = new CacheKey("Nop.plugins.widgets.myslider.slider-{0}-{1}-{2}-{3}", MY_SLIDER_PATTERN_KEY);
        public const string MY_SLIDER_PATTERN_KEY = "Nop.plugins.widgets.myslider.slider";

        public static CacheKey MY_SLIDER_ITEM_MODEL_KEY = new CacheKey("Nop.plugins.widgets.myslider.slider.item-{0}", MY_SLIDER_ITEM_PATTERN_KEY);
        public const string MY_SLIDER_ITEM_PATTERN_KEY = "Nop.plugins.widgets.myslider.slider.item";

        public static CacheKey PublicComponentKey => new CacheKey("Nop.plugins.widgets.myslider.Public.Component-{0}-{1}", PublicComponenPrefixCacheKey);
        public static string PublicComponenPrefixCacheKey => "Nop.plugins.widgets.myslider.Public.Component";

        private readonly IStaticCacheManager _staticCacheManager;

        public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }
        public async Task HandleEventAsync(EntityInsertedEvent<Setting> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
        }
        
        public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
        }
       
        public async Task HandleEventAsync(EntityDeletedEvent<Setting> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(PICTURE_URL_PATTERN_KEY);
        }

        public async Task HandleEventAsync(EntityInsertedEvent<MySliders> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_PATTERN_KEY);
        }

        public async Task HandleEventAsync(EntityUpdatedEvent<MySliders> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_PATTERN_KEY);
        }
        public async Task HandleEventAsync(EntityDeletedEvent<MySliders> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_PATTERN_KEY);
        }

        public async Task HandleEventAsync(EntityInsertedEvent<MySliderItem> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_ITEM_PATTERN_KEY);
        }

        public async Task HandleEventAsync(EntityUpdatedEvent<MySliderItem> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_ITEM_PATTERN_KEY);
        }

        public async Task HandleEventAsync(EntityDeletedEvent<MySliderItem> eventMessage)
        {
            await _staticCacheManager.RemoveByPrefixAsync(MY_SLIDER_ITEM_PATTERN_KEY);
        }
    }
}