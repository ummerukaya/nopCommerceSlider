using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Plugin.Widgets.MySlider.Services;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.MySlider.Factories
{
    public class MySliderModelFactory : IMySliderModelFactory
    {
        private readonly IPictureService _pictureService;
        private readonly IMySliderService _sliderService;
        private readonly IWorkContext _workContext;
      

        public MySliderModelFactory(IPictureService pictureService,
            IMySliderService sliderService,
            IWorkContext workContext
            )
        {
            _pictureService = pictureService;
            _sliderService = sliderService;
            _workContext = workContext;
           
        }
        public async Task<IList<SliderModel>> PrepareSliderListModelAsync(List<MySliders> sliders)
        {
            if (sliders == null)
                throw new ArgumentNullException(nameof(sliders));

            var model = new List<SliderModel>();
            foreach (var slider in sliders)
            {
                model.Add(await PrepareSliderModelAsync(slider));
            }
            return model;
        }

        public async Task<SliderModel> PrepareSliderModelAsync(MySliders slider)
        {
            var sliderModel = new SliderModel()
            {
                Id = slider.Id,
                WidgetZoneId = slider.WidgetZoneId,
                Nav = slider.Nav,
                AutoPlayHoverPause = slider.AutoPlayHoverPause,
                StartPosition = slider.StartPosition,
                LazyLoad = slider.LazyLoad,
                LazyLoadEager = slider.LazyLoadEager,
                AnimateOut = slider.AnimateOut,
                AnimateIn = slider.AnimateIn,
                Loop = slider.Loop,
                Margin = slider.Margin,
                OverrideGlobalSettings = slider.OverrideGlobalSettings,
                AutoPlay = slider.AutoPlay,
                AutoPlayTimeout = slider.AutoPlayTimeout,
                RTL = (await _workContext.GetWorkingLanguageAsync()).Rtl
            };

            //if (slider.WidgetZoneId != 5 && slider.WidgetZoneId != 7 && slider.WidgetZoneId != 8)
            //    sliderModel.BackGroundPictureUrl = await _pictureService.GetPictureUrlAsync(slider.BackGroundPictureId);

            var sliderItems = await _sliderService.GetSliderItemsBySliderIdAsync(slider.Id);
            foreach (var si in sliderItems)
            {
                sliderModel.Items.Add(new SliderModel.SliderItemModel()
                {
                    Id = si.Id,
                    Title = si.Title,
                    Link = si.Link,
                    ShopNowLink = si.ShopNowLink,
                    PictureUrl = await _pictureService.GetPictureUrlAsync(si.PictureId),
                    ImageAltText = si.ImageAltText,
                    ShortDescription = si.ShortDescription
                });
            }

            return sliderModel;
        }
    }
}
