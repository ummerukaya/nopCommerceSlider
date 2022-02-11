using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Services.Logging;

namespace Nop.Plugin.Widgets.MySlider.Helpers
{
    public class MySliderHelper
    {
        public static string[] WidgetZones = {"home_page_before_best_sellers","home_page_before_news","home_page_before_poll",
            "home_page_before_products","home_page_top","viridi_before_footer_nav","ThemeKallesHomepageProductItemsBefore",
            "ThemeKallesHomepageProductItemsAfter","valley_top_add_1","valley_top_add_2"};

        public static async Task<List<string>> GetCustomWidgetZonesAsync()
        {
            var zones = new List<string>();
            try
            {
                var list = await GetCustomWidgetZoneNameValuesAsync();

                if (list != null && list.Any())
                    zones.AddRange(list.Select(x => x.WidgetName).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct());
            }
            catch (Exception ex)
            {
                await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
            }
            return zones;
        }

        public static async Task<IList<SelectListItem>> GetCustomWidgetZoneSelectListAsync()
        {
            var list = new List<SelectListItem>();
            var zones = await GetCustomWidgetZoneNameValuesAsync();
            if (zones != null && zones.Any())
            {
                foreach (var item in zones)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = item.WidgetId.ToString(),
                        Text = item.WidgetName
                    });
                }
            }
            return list;
        }

        public static async Task<List<WidgetZoneModel>> GetCustomWidgetZoneNameValuesAsync()
        {
            var zones = new List<WidgetZoneModel>();
            try
            {
                int x = 1;
                foreach (var widgets in WidgetZones)
                {
                    zones.Add(new WidgetZoneModel()
                    {
                        WidgetId = x++,
                        WidgetName = widgets
                    });
                }
            }
            catch (Exception ex)
            {
                await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
            }
            return zones;
        }

        public static string GetCustomWidgetZone(int widgetZoneId)
        {
            var zones = GetCustomWidgetZoneNameValuesAsync().Result;
            if (zones != null && zones.Any(x => x.WidgetId == widgetZoneId))
            {
                return zones.FirstOrDefault(x => x.WidgetId == widgetZoneId).WidgetName;
            }
            return null;
        }
    }
}
