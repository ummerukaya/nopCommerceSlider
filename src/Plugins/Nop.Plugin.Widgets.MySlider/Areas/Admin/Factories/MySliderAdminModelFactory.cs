using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Configuration;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Factories;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Factories
{
    public class MySliderModelFactory : IMySliderModelFactory
    {
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IMySliderService _mysliderService;


        public MySliderModelFactory(IStoreContext storeContext,
            IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
            ILocalizedModelFactory localizedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            ILocalizationService localizationService,
            IPictureService pictureService,
            ISettingService settingService,
            IDateTimeHelper dateTimeHelper,
            IMySliderService mysliderService)
        {
            _storeContext = storeContext;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
            _localizedModelFactory = localizedModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _settingService = settingService;
            _dateTimeHelper = dateTimeHelper;
            _mysliderService = mysliderService;
        }

    }
}
