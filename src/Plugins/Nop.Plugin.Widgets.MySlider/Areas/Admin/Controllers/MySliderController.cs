using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Factories;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Plugin.Widgets.MySlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Controllers
{
    public class MySliderController : BaseAdminController
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
        private readonly IMySliderCustomerService _mysliderCustomerService;
        private readonly IStoreService _storeService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAclService _aclService;
        private readonly ICustomerService _customerService;

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
           IMySliderCustomerService mysliderCustomerService,
           IStoreService storeService,
           IStaticCacheManager staticCacheManager,
           ICustomerService customerService,
           IAclService aclService)
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
            _mysliderCustomerService = mysliderCustomerService;
            _storeService = storeService;
            _staticCacheManager = staticCacheManager;
            _aclService = aclService;
            _customerService = customerService;
        }
        protected async virtual Task SaveStoreMappingsAsync(MySliders slider, MySliderModel model)
        {
            slider.LimitedToStores = model.SelectedStoreIds.Any();

            var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(slider);
            var allStores = await _storeService.GetAllStoresAsync();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        await _storeMappingService.InsertStoreMappingAsync(slider, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        await _storeMappingService.DeleteStoreMappingAsync(storeMappingToDelete);
                }
            }
        }

        // Create / update / delete slider

        public async Task<IActionResult> CreateAsync()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var model = await _sliderModelFactory.PrepareSliderModelAsync(new MySliderModel(), null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> CreateAsync(MySliderModel model, bool continueEditing)
        {
            if (!(await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets)))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var slider = model.ToEntity<MySliders>();
                slider.CreatedOnUtc = DateTime.UtcNow;
                slider.UpdatedOnUtc = DateTime.UtcNow;
               
                await _mysliderService.InsertSliderAsync(slider);

                await SaveStoreMappingsAsync(slider, model);

                await _mysliderService.UpdateSliderAsync(slider);

                await _mysliderCustomerService.InsertCustomerRoleBySliderId(slider.Id, model.SelectedCustomerRoleIds);

                await _staticCacheManager.RemoveByPrefixAsync(ModelCacheEventConsumer.PublicComponenPrefixCacheKey);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.MySlider.Sliders.Created"));

                return continueEditing
                    ? RedirectToAction("Edit", new { id = slider.Id })
                    : RedirectToAction("List");
            }
            model = await _sliderModelFactory.PrepareSliderModelAsync(model, null);
            return View(model);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var slider = await _mysliderService.GetSliderByIdAsync(id);
            if (slider == null || slider.Deleted)
                return RedirectToAction("List");
            

            var model = await _sliderModelFactory.PrepareSliderModelAsync(null, slider);
           // model.SelectedCustomerRoleIds = _mysliderCustomerService.GetCustomerRoleBySliderId(slider.Id);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async virtual Task<IActionResult> EditAsync(MySliderModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var slider = await _mysliderService.GetSliderByIdAsync(model.Id);
            if (slider == null || slider.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                slider = model.ToEntity(slider);
                slider.UpdatedOnUtc = DateTime.UtcNow;
                if (model.CatalogPageId == 1)
                {
                    slider.WidgetZoneId = model.ProductWidgetZoneId;
                }
                else if (model.CatalogPageId == 2)
                {
                    slider.WidgetZoneId = model.CategoryWidgetZoneId;
                }
                else
                {
                    slider.WidgetZoneId = model.ManufactureWidgetZoneId;
                }
                await _mysliderService.UpdateSliderAsync(slider);
             
                await SaveStoreMappingsAsync(slider, model);

                await _mysliderCustomerService.InsertCustomerRoleBySliderId(slider.Id, model.SelectedCustomerRoleIds);
                await _mysliderService.UpdateSliderAsync(slider);
                await _staticCacheManager.RemoveByPrefixAsync(ModelCacheEventConsumer.PublicComponenPrefixCacheKey);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.MySlider.Sliders.Updated"));

                return continueEditing
                    ? RedirectToAction("Edit", new { id = model.Id })
                    : RedirectToAction("List");
            }

            model = await _sliderModelFactory.PrepareSliderModelAsync(model, slider);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var slider = await _mysliderService.GetSliderByIdAsync(id);
            if (slider == null || slider.Deleted)
                return RedirectToAction("List");

            await _mysliderService.DeleteSliderAsync(slider);
            await _staticCacheManager.RemoveByPrefixAsync(ModelCacheEventConsumer.PublicComponenPrefixCacheKey);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.MySlider.Sliders.Deleted"));

            return RedirectToAction("List");
        }


        //list
        public async Task<IActionResult> ListAsync()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var model = await _sliderModelFactory.PrepareSliderSearchModelAsync(new MySliderSearchModel());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListAsync(MySliderSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            var sliders = await _sliderModelFactory.PrepareSliderListModelAsync(searchModel);
           // var model = await RenderPartialViewToStringAsync("view", sliders);
            return Json(sliders);
        }


        //configure
        public async Task<IActionResult> ConfigureAsync()
        {
            if (!(await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets)))
                return AccessDeniedView();

            var model = await _sliderModelFactory.PrepareConfigurationModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfigureAsync(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var sliderSettings = await _settingService.LoadSettingAsync<MySliderSettings>(storeScope);
            sliderSettings.SelectedCustomerRoleIds = string.Join(",", model.SelectedCustomerRoleIds.ToArray());
            sliderSettings = model.ToSettings(sliderSettings);
            

            await _settingService.SaveSettingOverridablePerStoreAsync(sliderSettings, x => x.EnableSlider, 
                model.EnableSlider_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(sliderSettings, x => x.SelectedCustomerRoleIds,
                model.SelectedCustomerRoleIds_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.MySlider.Configuration.Updated"));

            return RedirectToAction("Configure");
        }

      

      

        // delete, create , update slider items
        public async virtual Task<IActionResult> SliderItemCreatePopupAsync(int sliderId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var slider = await _mysliderService.GetSliderByIdAsync(sliderId)
                ?? throw new ArgumentException("No slider found with the specified id", nameof(sliderId));

            //prepare model
            var model = await _sliderModelFactory.PrepareSliderItemModelAsync(new MySliderItemModel(), slider, null);

            return View(model);
        }

        [HttpPost]
        public async virtual Task<IActionResult> SliderItemCreatePopupAsync(MySliderItemModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a slider with the specified id
            var slider = await _mysliderService.GetSliderByIdAsync(model.MySlidersId)
                ?? throw new ArgumentException("No slider found with the specified id");

            if (ModelState.IsValid)
            {
                //fill entity from model
                var sliderItem = model.ToEntity<MySliderItem>();

                await _mysliderService.InsertSliderItemAsync(sliderItem);
                

                ViewBag.RefreshPage = true;
                await _staticCacheManager.RemoveByPrefixAsync(ModelCacheEventConsumer.PublicComponenPrefixCacheKey);
                return View(model);
            }

            //prepare model
            model = await _sliderModelFactory.PrepareSliderItemModelAsync(model, slider, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public async virtual Task<IActionResult> SliderItemEditPopupAsync(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a predefined slider value with the specified id
            var mysliderItem = await _mysliderService.GetSliderItemByIdAsync(id)
                ?? throw new ArgumentException("No slider item found with the specified id");

            //try to get a slider with the specified id
            var slider = await _mysliderService.GetSliderByIdAsync(mysliderItem.MySlidersId)
                ?? throw new ArgumentException("No slider found with the specified id");

            //prepare model
            var model = await _sliderModelFactory.PrepareSliderItemModelAsync(null, slider, mysliderItem);

            return View(model);
        }

        [HttpPost]
        public async virtual Task<IActionResult> SliderItemEditPopupAsync(MySliderItemModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //try to get a predefined slider value with the specified id
            var sliderItem = await _mysliderService.GetSliderItemByIdAsync(model.Id)
                ?? throw new ArgumentException("No slider item found with the specified id");

            //try to get a slider with the specified id
            var slider = await _mysliderService.GetSliderByIdAsync(sliderItem.MySlidersId)
                ?? throw new ArgumentException("No slider found with the specified id");

            if (ModelState.IsValid)
            {
                sliderItem = model.ToEntity(sliderItem);
                sliderItem.Title = model.SliderItemTitle;
                await _mysliderService.UpdateSliderItemAsync(sliderItem);

                
                ViewBag.RefreshPage = true;
                await _staticCacheManager.RemoveByPrefixAsync(ModelCacheEventConsumer.PublicComponenPrefixCacheKey);
                return View(model);
            }

            //prepare model
            model = await _sliderModelFactory.PrepareSliderItemModelAsync(model, slider, sliderItem, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SliderItemDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            var sliderItem = await _mysliderService.GetSliderItemByIdAsync(id)
                ?? throw new ArgumentException("No slider item found with the specified id");

            var slider = await _mysliderService.GetSliderByIdAsync(sliderItem.MySlidersId);
            if (slider.Deleted)
                return new NullJsonResult();

            await _mysliderService.DeleteSliderItemAsync(sliderItem);

            return new NullJsonResult();
        }

        [HttpPost]
        public async Task<IActionResult> SliderItemList(MySliderItemSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return await AccessDeniedDataTablesJson();

            var listModel = await _sliderModelFactory.PrepareSliderItemListModelAsync(searchModel);
            return Json(listModel);
        }
    }
}
