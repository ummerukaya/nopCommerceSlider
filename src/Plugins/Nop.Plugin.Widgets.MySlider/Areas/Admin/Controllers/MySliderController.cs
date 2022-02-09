using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Factories;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Controllers
{
    public class MySliderController : BaseNopController
    {
        private readonly IStoreContext _storeContext;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IMySliderAdminModelFactory _sliderModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly MySliderSettings _mysliderSettings;
        private readonly IMySliderService _mysliderService;
        private readonly IStoreService _storeService;
        private readonly IStaticCacheManager _staticCacheManager;

        public MySliderController(IStoreContext storeContext,
           ILocalizedEntityService localizedEntityService,
           ILocalizationService localizationService,
           INotificationService notificationService,
           IStoreMappingService storeMappingService,
           IMySliderAdminModelFactory sliderModelFactory,
           IPermissionService permissionService,
           IPictureService pictureService,
           ISettingService settingService,
           MySliderSettings mysliderSettings,
           IMySliderService mysliderService,
           IStoreService storeService,
           IStaticCacheManager staticCacheManager)
        {
            _storeContext = storeContext;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _storeMappingService = storeMappingService;
            _sliderModelFactory = sliderModelFactory;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _mysliderSettings = mysliderSettings;
            _settingService = settingService;
            _mysliderService = mysliderService;
            _storeService = storeService;
            _staticCacheManager = staticCacheManager;
        }

        

    }
}
