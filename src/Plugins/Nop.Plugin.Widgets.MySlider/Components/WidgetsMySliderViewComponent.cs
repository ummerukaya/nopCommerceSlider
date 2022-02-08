using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.MySlider.Infrastructure.Cache;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
namespace Nop.Plugin.Widgets.MySlider.Components
{

    [ViewComponent(Name = "WidgetsMySlider")]
    public class WidgetsMySliderViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        public WidgetsMySliderViewComponent(IStoreContext storeContext,
            IStaticCacheManager staticCacheManager,
            ISettingService settingService,
            IPictureService pictureService,
            IWebHelper webHelper)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
        }

       
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            
            var mySliderSettings = await _settingService.LoadSettingAsync<MySliderSettings>((await _storeContext.GetCurrentStoreAsync()).Id);

            var model = new PublicInfoModel
            {
                Picture1Url = await GetPictureUrlAsync(mySliderSettings.Picture1Id),
                Text1 = mySliderSettings.Text1,
                Link1 = mySliderSettings.Link1,
                AltText1 = mySliderSettings.AltText1,

                Picture2Url = await GetPictureUrlAsync(mySliderSettings.Picture2Id),
                Text2 = mySliderSettings.Text2,
                Link2 = mySliderSettings.Link2,
                AltText2 = mySliderSettings.AltText2,

                Picture3Url = await GetPictureUrlAsync(mySliderSettings.Picture3Id),
                Text3 = mySliderSettings.Text3,
                Link3 = mySliderSettings.Link3,
                AltText3 = mySliderSettings.AltText3,

                Picture4Url = await GetPictureUrlAsync(mySliderSettings.Picture4Id),
                Text4 = mySliderSettings.Text4,
                Link4 = mySliderSettings.Link4,
                AltText4 = mySliderSettings.AltText4,

                Picture5Url = await GetPictureUrlAsync(mySliderSettings.Picture5Id),
                Text5 = mySliderSettings.Text5,
                Link5 = mySliderSettings.Link5,
                AltText5 = mySliderSettings.AltText5

            };

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                return Content("");

            return View("~/Plugins/Widgets.MySlider/Views/PublicInfo.cshtml", model);
        }

        protected async Task<string> GetPictureUrlAsync(int pictureId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY,
                pictureId, _webHelper.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                //little hack here. nulls aren't cacheable so set it to ""
                var url = await _pictureService.GetPictureUrlAsync(pictureId, showDefaultPicture: false) ?? "";
                return url;
            });
        }
    }
}
