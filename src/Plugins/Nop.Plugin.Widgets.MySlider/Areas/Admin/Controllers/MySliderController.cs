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
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
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

        // Create / update / delete

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

                await _mysliderService.UpdateSliderAsync(slider);

               
                await SaveStoreMappingsAsync(slider, model);

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
            return Json(sliders);
        }
    }
}
