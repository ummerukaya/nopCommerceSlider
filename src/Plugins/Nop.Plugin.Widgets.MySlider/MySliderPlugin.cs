using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.MySlider.Helpers;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.MySlider
{
    public class MySliderPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly INopFileProvider _fileProvider;
        public MySliderPlugin(ILocalizationService localizationService,
            IPictureService pictureService,
            ISettingService settingService,
            IWebHelper webHelper,
            INopFileProvider fileProvider)
        {
            _localizationService = localizationService;
            _pictureService = pictureService;
            _settingService = settingService;
            _webHelper = webHelper;
            _fileProvider = fileProvider;
        }
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MySlider/Configure";
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            //if (widgetZone == PublicWidgetZones.HeadHtmlTag)
            //    return "MySliderHeadHtmlTag";

            return "MySlider";
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            var widgetZones = await MySliderHelper.GetCustomWidgetZonesAsync();
            widgetZones.Add(PublicWidgetZones.HeadHtmlTag);

            return widgetZones;
        }


        public async override Task InstallAsync()
        {
            await base.InstallAsync();
            await CreateSampleDataAsync();
        }

        private Task CreateSampleDataAsync()
        {
            throw new NotImplementedException();
        }

        public async override Task UninstallAsync()
        {
            await base.UninstallAsync();
        }
   

        public bool HideInWidgetList => true;
    }
}
