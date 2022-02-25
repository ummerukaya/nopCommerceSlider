using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.MySlider.Factories;
using Nop.Plugin.Widgets.MySlider.Helpers;
using Nop.Plugin.Widgets.MySlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
namespace Nop.Plugin.Widgets.MySlider.Components
{

    [ViewComponent(Name = "MySlider")]
    public class MySliderViewComponent : NopViewComponent
    {
        
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IMySliderModelFactory _sliderModelFactory;
        private readonly ISettingService _settingService;
        private readonly MySliderSettings _sliderSettings;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;
        private readonly IMySliderService _sliderService;
        private readonly IMySliderCustomerService _sliderCustomerService;
        private readonly IWorkContext _workContext;

        public MySliderViewComponent(IStoreContext storeContext,
            IStaticCacheManager staticCacheManager,
            ISettingService settingService,
            IPictureService pictureService,
            IMySliderModelFactory sliderModelFactory,
            IMySliderService sliderService,
            IMySliderCustomerService sliderCustomerService,
            MySliderSettings sliderSettings,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _sliderService = sliderService;
            _pictureService = pictureService;
            _sliderModelFactory = sliderModelFactory;
            _webHelper = webHelper;
            _sliderSettings = sliderSettings;
            _workContext = workContext;
            _sliderCustomerService = sliderCustomerService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone,object additionalData)
        {
            if (_sliderSettings.EnableSlider)
            {
                if (!MySliderHelper.TryGetWidgetZoneId(widgetZone, out int widgetZoneId))
                    return Content("");

                var currentCustomer = await _workContext.GetCurrentCustomerAsync();
                var currentCustomerRoleIds = _sliderCustomerService.GetCurrentCustomerRoleIds(currentCustomer.Id);

                
                var customerRoleIds = MySliderHelper.GetGlobalCustomerRoleIds(_sliderSettings.SelectedCustomerRoleIds);

                //if (!MySliderHelper.ValidateCustomerByRoleIds(customerRoleIds, currentCustomerRoleIds))
                //    return Content("");

                IList<SliderModel> sliderModels = new List<SliderModel>();
                var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheEventConsumer.PublicComponentKey,
                                                                          widgetZone, 
                                                                          (await _storeContext.GetCurrentStoreAsync()).Id);

                sliderModels = await _staticCacheManager.GetAsync(cacheKey, async () =>
                {
                    var sliders = (await _sliderService.GetAllSlidersAsync(new List<int> { widgetZoneId },
                        storeId: (await _storeContext.GetCurrentStoreAsync()).Id,
                        active: true)).ToList();

                    var model = sliders.Count() > 0 ? await _sliderModelFactory.PrepareSliderListModelAsync(sliders) : new List<Models.SliderModel>();

                    return model;
                });

                for (int i=0;i<sliderModels.Count();i++)
                {
                    if (sliderModels[i].OverrideGlobalSettings)
                    {
                        customerRoleIds = _sliderCustomerService.GetCustomerRoleBySliderId(sliderModels[i].Id);
                    }

                    if (!MySliderHelper.ValidateCustomerByRoleIds(customerRoleIds, currentCustomerRoleIds))
                        sliderModels[i] = null;
                }

                return View(sliderModels);
            }
            else
            {
                return Content("");
            }
        }

    }
}
