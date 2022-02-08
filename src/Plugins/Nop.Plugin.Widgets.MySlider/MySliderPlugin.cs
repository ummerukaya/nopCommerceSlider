using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

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
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsMySlider";
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> {PublicWidgetZones.HomepageBeforeNews});
        }

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsMySlider/Configure";
        }

        public override async Task InstallAsync()
        {
            var sampleImagesPath = _fileProvider.MapPath("~/Plugins/Widgets.MySlider/Content/myslider/sample-images/");
            var settings = new MySliderSettings
            {
                Picture1Id = (await _pictureService.InsertPictureAsync(await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "banner1.jpg")), MimeTypes.ImagePJpeg, "banner_1")).Id,
                Text1 = "",
                Link1 = _webHelper.GetStoreLocation(),
                Picture2Id = (await _pictureService.InsertPictureAsync(await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "banner2.jpg")), MimeTypes.ImagePJpeg, "banner_2")).Id,
                Text2 = "",
                Link2 = _webHelper.GetStoreLocation()
            };

            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.MySlider.Picture1"] = "Picture 1",
                ["Plugins.Widgets.MySlider.Picture2"] = "Picture 2",
                ["Plugins.Widgets.MySlider.Picture3"] = "Picture 3",
                ["Plugins.Widgets.MySlider.Picture4"] = "Picture 4",
                ["Plugins.Widgets.MySlider.Picture5"] = "Picture 5",
                ["Plugins.Widgets.MySlider.Picture"] = "Picture",
                ["Plugins.Widgets.MySlider.Picture.Hint"] = "Upload picture.",
                ["Plugins.Widgets.MySlider.Text"] = "Comment",
                ["Plugins.Widgets.MySlider.Text.Hint"] = "Enter comment for picture. Leave empty if you don't want to display any text.",
                ["Plugins.Widgets.MySlider.Link"] = "URL",
                ["Plugins.Widgets.MySlider.Link.Hint"] = "Enter URL. Leave empty if you don't want this picture to be clickable.",
                ["Plugins.Widgets.MySlider.AltText"] = "Image alternate text",
                ["Plugins.Widgets.MySlider.AltText.Hint"] = "Enter alternate text that will be added to image."
            });

            await base.InstallAsync();
        }


        public override async Task UninstallAsync()
        {
            
            await _settingService.DeleteSettingAsync<MySliderSettings>();

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.MySlider");

            await base.UninstallAsync();
        }
    }
}
