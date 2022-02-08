using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.MySlider.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsMySliderController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public WidgetsMySliderController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService, 
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _storeContext = storeContext;
        }


        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
       
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var mySliderSettings = await _settingService.LoadSettingAsync<MySliderSettings>(storeScope);
            var model = new ConfigurationModel
            {
                Picture1Id = mySliderSettings.Picture1Id,
                Text1 = mySliderSettings.Text1,
                Link1 = mySliderSettings.Link1,
                AltText1 = mySliderSettings.AltText1,
                Picture2Id = mySliderSettings.Picture2Id,
                Text2 = mySliderSettings.Text2,
                Link2 = mySliderSettings.Link2,
                AltText2 = mySliderSettings.AltText2,
                Picture3Id = mySliderSettings.Picture3Id,
                Text3 = mySliderSettings.Text3,
                Link3 = mySliderSettings.Link3,
                AltText3 = mySliderSettings.AltText3,
                Picture4Id = mySliderSettings.Picture4Id,
                Text4 = mySliderSettings.Text4,
                Link4 = mySliderSettings.Link4,
                AltText4 = mySliderSettings.AltText4,
                Picture5Id = mySliderSettings.Picture5Id,
                Text5 = mySliderSettings.Text5,
                Link5 = mySliderSettings.Link5,
                AltText5 = mySliderSettings.AltText5,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Link1, storeScope);
                model.AltText1_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.AltText1, storeScope);
                model.Picture2Id_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Link2, storeScope);
                model.AltText2_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.AltText2, storeScope);
                model.Picture3Id_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Link3, storeScope);
                model.AltText3_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.AltText3, storeScope);
                model.Picture4Id_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Link4, storeScope);
                model.AltText4_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.AltText4, storeScope);
                model.Picture5Id_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.Link5, storeScope);
                model.AltText5_OverrideForStore = await _settingService.SettingExistsAsync(mySliderSettings, x => x.AltText5, storeScope);
            }

            return View("~/Plugins/Widgets.MySlider/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var mySliderSettings = await _settingService.LoadSettingAsync<MySliderSettings>(storeScope);

           
            var previousPictureIds = new[] 
            {
                mySliderSettings.Picture1Id,
                mySliderSettings.Picture2Id,
                mySliderSettings.Picture3Id,
                mySliderSettings.Picture4Id,
                mySliderSettings.Picture5Id
            };

            mySliderSettings.Picture1Id = model.Picture1Id;
            mySliderSettings.Text1 = model.Text1;
            mySliderSettings.Link1 = model.Link1;
            mySliderSettings.AltText1 = model.AltText1;
            mySliderSettings.Picture2Id = model.Picture2Id;
            mySliderSettings.Text2 = model.Text2;
            mySliderSettings.Link2 = model.Link2;
            mySliderSettings.AltText2 = model.AltText2;
            mySliderSettings.Picture3Id = model.Picture3Id;
            mySliderSettings.Text3 = model.Text3;
            mySliderSettings.Link3 = model.Link3;
            mySliderSettings.AltText3 = model.AltText3;
            mySliderSettings.Picture4Id = model.Picture4Id;
            mySliderSettings.Text4 = model.Text4;
            mySliderSettings.Link4 = model.Link4;
            mySliderSettings.AltText4 = model.AltText4;
            mySliderSettings.Picture5Id = model.Picture5Id;
            mySliderSettings.Text5 = model.Text5;
            mySliderSettings.Link5 = model.Link5;
            mySliderSettings.AltText5 = model.AltText5;


            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Picture1Id, model.Picture1Id_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Text1, model.Text1_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Link1, model.Link1_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.AltText1, model.AltText1_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Picture2Id, model.Picture2Id_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Text2, model.Text2_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Link2, model.Link2_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.AltText2, model.AltText2_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Picture3Id, model.Picture3Id_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Text3, model.Text3_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Link3, model.Link3_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.AltText3, model.AltText3_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Picture4Id, model.Picture4Id_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Text4, model.Text4_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Link4, model.Link4_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.AltText4, model.AltText4_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Picture5Id, model.Picture5Id_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Text5, model.Text5_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.Link5, model.Link5_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(mySliderSettings, x => x.AltText5, model.AltText5_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();
            
            var currentPictureIds = new[]
            {
                mySliderSettings.Picture1Id,
                mySliderSettings.Picture2Id,
                mySliderSettings.Picture3Id,
                mySliderSettings.Picture4Id,
                mySliderSettings.Picture5Id
            };

            
            foreach (var pictureId in previousPictureIds.Except(currentPictureIds))
            { 
                var previousPicture = await _pictureService.GetPictureByIdAsync(pictureId);
                if (previousPicture != null)
                    await _pictureService.DeletePictureAsync(previousPicture);
            }

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            
            return await Configure();
        }
    }
}