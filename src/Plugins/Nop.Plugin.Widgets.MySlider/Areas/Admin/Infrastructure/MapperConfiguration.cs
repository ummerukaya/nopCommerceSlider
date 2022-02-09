using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Plugin.Widgets.MySlider.Models;

namespace Nop.Plugin.Widgets.MySlider.Areas.Admin.Infrastructure
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        public int Order => 1;

        public MapperConfiguration()
        {
            CreateMap<MySliderSettings, ConfigurationModel>()
                   .ForMember(model => model.EnableSlider_OverrideForStore, options => options.Ignore())
                   .ForMember(model => model.CustomProperties, options => options.Ignore())
                   .ForMember(model => model.ActiveStoreScopeConfiguration, options => options.Ignore());
            CreateMap<ConfigurationModel, MySliderSettings>();

            CreateMap<MySliders, MySliderModel>()
                    .ForMember(model => model.AvailableStores, options => options.Ignore())
                    .ForMember(model => model.AvailableWidgetZones, options => options.Ignore())
                    .ForMember(model => model.WidgetZoneStr, options => options.Ignore())
                    .ForMember(model => model.CreatedOn, options => options.Ignore())
                    .ForMember(model => model.UpdatedOn, options => options.Ignore())
                    .ForMember(model => model.SliderItemSearchModel, options => options.Ignore())
                    .ForMember(model => model.CustomProperties, options => options.Ignore())
                    .ForMember(model => model.SelectedStoreIds, options => options.Ignore());
            CreateMap<MySliderModel, MySliders>()
                    .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                    .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

            CreateMap<MySliderItem, MySliderItemModel>()
                    .ForMember(model => model.CustomProperties, options => options.Ignore())
                    .ForMember(model => model.FullPictureUrl, options => options.Ignore())
                    .ForMember(model => model.PictureUrl, options => options.Ignore())
                    .ForMember(model => model.MobileFullPictureUrl, options => options.Ignore())
                    .ForMember(model => model.MobilePictureUrl, options => options.Ignore());
            CreateMap<MySliderItemModel, MySliderItem>();
        }
    }
}
