using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Plugin.Widgets.MySlider.Helpers;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Configuration;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Factories
{
    public class MySliderAdminModelFactory : IMySliderAdminModelFactory
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


        public MySliderAdminModelFactory(IStoreContext storeContext,
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

        public async Task<ConfigurationModel> PrepareConfigurationModelAsync()
        {
            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var sliderSettings = await _settingService.LoadSettingAsync<MySliderSettings>(storeId);

            var model = sliderSettings.ToSettingsModel<ConfigurationModel>();

            model.ActiveStoreScopeConfiguration = storeId;

            if (storeId <= 0)
                return model;

            model.EnableSlider_OverrideForStore = await _settingService.SettingExistsAsync(sliderSettings, x => x.EnableSlider, storeId);

            return model;
        }


        //Slider Item Model
        public async Task<MySliderItemModel> PrepareSliderItemModelAsync(MySliderItemModel model, MySliders slider, MySliderItem sliderItem, bool excludeProperties = false)
        {
            
            if (sliderItem != null)
            {
                if (model == null)
                {
                    model = sliderItem.ToModel<MySliderItemModel>();
                    model.PictureUrl = await _pictureService.GetPictureUrlAsync(sliderItem.PictureId, 200);
                    model.FullPictureUrl = await _pictureService.GetPictureUrlAsync(sliderItem.PictureId);
                    model.MobilePictureUrl = await _pictureService.GetPictureUrlAsync(sliderItem.MobilePictureId, 200);
                    model.MobileFullPictureUrl = await _pictureService.GetPictureUrlAsync(sliderItem.MobilePictureId);
                    model.SliderItemTitle = sliderItem.Title;
                }

            }

            model.SliderId = slider.Id;

            return model;
        }


        //slider item list model
        public async Task<MySliderItemListModel> PrepareSliderItemListModelAsync(MySliderItemSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));


            var sliderItems = await _mysliderService.GetSliderItemsBySliderIdAsync(searchModel.SliderId, searchModel.Page - 1, searchModel.PageSize);


            var model = await new MySliderItemListModel().PrepareToGridAsync(searchModel, sliderItems, () =>
            {
                return sliderItems.SelectAwait(async sliderItem =>
                {
                    var slider = await _mysliderService.GetSliderByIdAsync(sliderItem.SliderId);
                    return await PrepareSliderItemModelAsync(null, slider, sliderItem);
                });
            });

            return model;
        }


        // slider model
        public async Task<MySliderModel> PrepareSliderModelAsync(MySliderModel model, MySliders slider, bool excludeProperties = false)
        {

            if (slider != null)
            {
                if (model == null)
                {
                    model = slider.ToModel<MySliderModel>();
                    model.WidgetZoneStr = MySliderHelper.GetCustomWidgetZone(slider.WidgetZoneId);
                    model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(slider.CreatedOnUtc, DateTimeKind.Utc);
                    model.UpdatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(slider.UpdatedOnUtc, DateTimeKind.Utc);
                }
            }

            if (!excludeProperties)
            {
                model.AvailableWidgetZones = await MySliderHelper.GetCustomWidgetZoneSelectListAsync();

                await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, slider, excludeProperties);
            }

            return model;
        }


        // Slider list model
        public async Task<MySliderListModel> PrepareSliderListModelAsync(MySliderSearchModel slidersearchModel)
        {
            if (slidersearchModel == null)
                throw new ArgumentNullException(nameof(slidersearchModel));

            var widgetZoneIds = slidersearchModel.SearchWidgetZones?.Contains(0) ?? true ? null : slidersearchModel.SearchWidgetZones.ToList();

            bool? active = null;
            if (slidersearchModel.SearchActiveId == 1)
                active = true;
            else if (slidersearchModel.SearchActiveId == 2)
                active = false;

            
            var sliders = await _mysliderService.GetAllSlidersAsync(widgetZoneIds, slidersearchModel.SearchStoreId,
                active, slidersearchModel.Page - 1, slidersearchModel.PageSize);

            
            var model = await new MySliderListModel().PrepareToGridAsync(slidersearchModel, sliders, () =>
            {
                return sliders.SelectAwait(async slider =>
                {
                    return await PrepareSliderModelAsync(null, slider, true);
                });
            });

            return model;
        }

        // slider search model
        public async Task<MySliderSearchModel> PrepareSliderSearchModelAsync(MySliderSearchModel sliderSearchModel)
        {
            if (sliderSearchModel == null)
                throw new ArgumentNullException(nameof(sliderSearchModel));

            await PrepareCustomWidgetZonesAsync(sliderSearchModel.AvailableWidgetZones, true);
            await PrepareActiveOptionsAsync(sliderSearchModel.AvailableActiveOptions, true);

            await _baseAdminModelFactory.PrepareStoresAsync(sliderSearchModel.AvailableStores);

            return sliderSearchModel;
        }



        private async Task PrepareActiveOptionsAsync(IList<SelectListItem> items, bool withSpecialDefaultItem = true)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items.Add(new SelectListItem()
            {
                Text = await _localizationService.GetResourceAsync("Nop.MySlider.Sliders.List.SearchActive.Active"),
                Value = "1"
            });
            items.Add(new SelectListItem()
            {
                Text = await _localizationService.GetResourceAsync("Nop.MySlider.Sliders.List.SearchActive.Inactive"),
                Value = "2"
            });

            if (withSpecialDefaultItem)
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                    Value = "0"
                });
            }
        }

        private async Task PrepareCustomWidgetZonesAsync(IList<SelectListItem> items, bool withSpecialDefaultItem = true)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var availableWidgetZones = await MySliderHelper.GetCustomWidgetZoneSelectListAsync();

            foreach (var zone in availableWidgetZones)
            {
                items.Add(zone);
            }

            if (withSpecialDefaultItem)
            {
                items.Insert(0, new SelectListItem()
                {
                    Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                    Value = "0"
                });
            }
        }
    }
}
