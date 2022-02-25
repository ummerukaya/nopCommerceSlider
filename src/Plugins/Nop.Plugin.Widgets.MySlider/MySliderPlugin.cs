using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Helpers;
using Nop.Plugin.Widgets.MySlider.Services;
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
        private readonly IMySliderService _sliderService;
        private readonly IWebHelper _webHelper;
        private readonly INopFileProvider _fileProvider;
        public MySliderPlugin(ILocalizationService localizationService,
            IPictureService pictureService,
            ISettingService settingService,
            IMySliderService sliderService,
            IWebHelper webHelper,
            INopFileProvider fileProvider)
        {
            _localizationService = localizationService;
            _pictureService = pictureService;
            _settingService = settingService;
            _sliderService = sliderService;
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
            //widgetZones.Add(PublicWidgetZones.HeadHtmlTag);

            return widgetZones;
        }


        public List<KeyValuePair<string, string>> PluginResouces()
        {
            var list = new List<KeyValuePair<string, string>>();

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchActive.Active", "Active"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchActive.Inactive", "Inactive"));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Menu.AnywhereSlider", "Anywhere slider"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Menu.Configuration", "Configuration"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Menu.List", "List"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Configuration", "Slider settings"));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Tab.Info", "Info"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Tab.Properties", "Properties"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Tab.SliderItems", "Slider items"));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderList", "Sliders"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.EditDetails", "Edit slider details"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.BackToList", "back to slider list"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.AddNew", "Add new slider"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.SaveBeforeEdit", "You need to save the slider before you can add items for this slider page."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.AddNew", "Add new item"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Pictures.Alert.PictureAdd", "Failed to add product picture."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Configuration.Fields.EnableSlider", "Enable slider"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Configuration.Fields.EnableSlider.Hint", "Check to enable slider for your store."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.sliders.Configuration.Updated", "Slider configuration updated successfully."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.sliders.Created", "Slider has been created successfully."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.sliders.Updated", "Slider has been updated successfully."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.sliders.Deleted", "Slider has been deleted successfully."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.DisplayOrder", "Display order"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Picture", "Picture"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.MobilePicture", "Mobile picture"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ImageAltText", "Alt"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Title", "Title"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ShortDescription", "Short description"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Link", "Link"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.DisplayOrder.Hint", "The display order for this slider item. 1 represents the top of the list."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Picture.Hint", "Picture of this slider item."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.MobilePicture.Hint", "Mobile view picture of this slider item."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ImageAltText.Hint", "Override \"alt\" attribute for \"img\" HTML element."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Title.Hint", "Override \"title\" attribute for \"img\" HTML element."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Link.Hint", "Custom link for slider item picture."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ShortDescription.Hint", "Short description for this slider item."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Sliders.Fields.Name", "Name"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Name.Hint", "The slider name."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Title", "Title"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Title.Hint", "The slider title."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.DisplayTitle", "Display title"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.DisplayTitle.Hint", "Determines whether title should be displayed on public site (depends on theme design)."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Sliders.Fields.Active", "Active"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Active.Hint", "Determines whether this slider is active (visible on public store)."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Sliders.Fields.WidgetZone", "Widget zone"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.WidgetZone.Hint", "The widget zone where this slider will be displayed."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Picture", "Picture"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Picture.Hint", "The slider picture."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CustomUrl", "Custom url"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CustomUrl.Hint", "The slider custom url."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlay", "Auto play"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlay.Hint", "Check to enable auto play."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CustomCssClass", "Custom css class"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CustomCssClass.Hint", "Enter the custom CSS class to be applied."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.DisplayOrder", "Display order"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.DisplayOrder.Hint", "Display order of the slider. 1 represents the top of the list."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Loop", "Loop"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Loop.Hint", "heck to enable loop."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Margin", "Margin"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Margin.Hint", "It's margin-right (px) on item."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.StartPosition", "Start position"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.StartPosition.Hint", "Starting position."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Center", "Center"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Center.Hint", "Check to center item. It works well with even and odd number of items."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Nav", "NAV"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Nav.Hint", "Check to enable next/prev buttons."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.LazyLoad", "Lazy load"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.LazyLoad.Hint", "Check to enable lazy load."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.LazyLoadEager", "Lazy load eager"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.LazyLoadEager.Hint", "Specify how many items you want to pre-load images to the right (and left when loop is enabled)."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlayTimeout", "Auto play timeout"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlayTimeout.Hint", "It's autoplay interval timeout. (e.g 5000)"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlayHoverPause", "Auto play hover pause"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AutoPlayHoverPause.Hint", "Check to enable pause on mouse hover."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AnimateOut", "Animate out"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AnimateOut.Hint", "Animate out."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AnimateIn", "Animate in"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.AnimateIn.Hint", "Animate in."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CreatedOn", "Created on"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.CreatedOn.Hint", "The create date of this slider."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.UpdatedOn", "Updated on"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.UpdatedOn.Hint", "The last update date of this slider."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.SelectedStoreIds", "Limited to stores"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.SelectedStoreIds.Hint", "Option to limit this slider to a certain store. If you have multiple stores, choose one or several from the list. If you don't use this option just leave this field empty."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Name.Required", "The name field is required."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.BackGroundPicture.Required", "The background picture is required."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchWidgetZones", "Widget zones"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchWidgetZones.Hint", "The search widget zones."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchStore", "Store"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchStore.Hint", "The search store."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchActive", "Active"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.List.SearchActive.Hint", "The search active."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.EditDetails", "Edit details"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Title.Required", "Title is required."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.Picture.Required", "Picture is required."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.MobilePicture.Required", "Title is required."));

            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Pictures.Alert.AddNew", "Upload picture first."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.BackgroundPicture", "Background picture"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.BackgroundPicture.Hint", "Background picture for this slider."));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.Fields.Name.Required", "Slider Name Is Required"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ShopNowLink", "ShopNow Link"));
            list.Add(new KeyValuePair<string, string>("Nop.MySlider.SliderItems.Fields.ShopNowLink.Hint", "Your ShopNow Link"));
            list.Add(new KeyValuePair<string, string>("MySlider.ShopNow", "Shop Now"));



            return list;
        }

        public async override Task InstallAsync()
        {
            await CreateSampleDataAsync();
            var keyValuePairs = PluginResouces();
            foreach (var keyValuePair in keyValuePairs)
            {
                await _localizationService.AddOrUpdateLocaleResourceAsync(keyValuePair.Key, keyValuePair.Value);
            }
            await base.InstallAsync();
        }

        private async Task CreateSampleDataAsync()
        {
            var sliderSetting = new MySliderSettings()
            {
                EnableSlider = true,
                SelectedCustomerRoleIds = "0"
            };
            await _settingService.SaveSettingAsync(sliderSetting);

            var slider = new MySliders()
            {
                Active = true,
                AutoPlay = true,
                AutoPlayTimeout = 3000,
                AutoPlayHoverPause = true,
                CreatedOnUtc = DateTime.UtcNow,
                Name = "Home page top",
                Loop = true,
                UpdatedOnUtc = DateTime.UtcNow,
                Nav = true,
                DisplayOrder = 0,
                StartPosition = 0,
                WidgetZoneId = 5,
                CatalogPageId = 1
            };
            await _sliderService.InsertSliderAsync(slider);

            var sampleImagesPath = _fileProvider.MapPath("~/Plugins/Widgets.MySlider/Content/sample/");
            var sliderItems = new MySliderItem()
            {
                PictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-1.jpg"))), MimeTypes.ImageJpeg, "slider-1")).Id,
                MobilePictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-1-mobile.jpg"))), MimeTypes.ImageJpeg, "slider-1")).Id,
                Title = "Liquid for Chicken",
                ShortDescription = "The Best General Tso's Chicken",
                MySlidersId = slider.Id
            };
            await _sliderService.InsertSliderItemAsync(sliderItems);

            var sliderItemsSecond = new MySliderItem()
            {
                PictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-2.jpg"))), MimeTypes.ImageJpeg, "slider-2")).Id,
                MobilePictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-2-mobile.jpg"))), MimeTypes.ImageJpeg, "slider-2")).Id,
                Title = "Pressure Cooker",
                ShortDescription = "Ribollita Into a Weeknight Meal",
                MySlidersId = slider.Id
            };
            await _sliderService.InsertSliderItemAsync(sliderItemsSecond);

            var sliderItemsThird = new MySliderItem()
            {
                PictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-3.jpg"))), MimeTypes.ImageJpeg, "slider-3")).Id,
                MobilePictureId = (await _pictureService.InsertPictureAsync((await _fileProvider.ReadAllBytesAsync(_fileProvider.Combine(sampleImagesPath, "slider-3-mobile.jpg"))), MimeTypes.ImageJpeg, "slider-3")).Id,
                Title = "Ingredients",
                ShortDescription = "The Best General Tso's Chicken",
                MySlidersId = slider.Id
            };
            await _sliderService.InsertSliderItemAsync(sliderItemsThird);
        }

        public async override Task UninstallAsync()
        {
            await base.UninstallAsync();
        }
   

        public bool HideInWidgetList => false;
    }
}
