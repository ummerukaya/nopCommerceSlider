using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.MySlider.Models;
using Nop.Services.Logging;
using Nop.Web.Areas.Admin.Models.Customers;

namespace Nop.Plugin.Widgets.MySlider.Helpers
{
    public class MySliderHelper
    {
        //public static string[] WidgetZones = {"home_page_before_best_sellers","home_page_before_news","home_page_before_poll",
        //    "home_page_before_products","home_page_top","viridi_before_footer_nav","ThemeKallesHomepageProductItemsBefore",
        //    "ThemeKallesHomepageProductItemsAfter","valley_top_add_1","valley_top_add_2"};
        //public static string[] WidgetZones = { 
        //      "productbox_addinfo_after","productbox_addinfo_before", "productbox_addinfo_middle","productbreadcrumb_after",
        //    "productbreadcrumb_before","productdetails_add_info","productdetails_after_breadcrumb","productdetails_after_pictures",
        //    "productdetails_before_collateral","productdetails_before_pictures","productdetails_bottom",
        //    "productdetails_essential_bottom","productdetails_essential_top", "productdetails_inside_overview_buttons_after",
        //    "productdetails_inside_overview_buttons_before", "productdetails_overview_bottom","productdetails_overview_top",
        //    "productdetails_top","productreviews_page_bottom", "productreviews_page_inside_review","productreviews_page_top",
        //    "productsbytag_before_product_list","productsbytag_bottom", "productsbytag_top", "productsearch_page_advanced",
        //    "productsearch_page_after_results", "productsearch_page_basic", "productsearch_page_before_results",
        //       "categorydetails_after_breadcrumb","categorydetails_after_featured_products",
        //    "categorydetails_before_featured_products", "categorydetails_before_filters", "categorydetails_before_product_list",
        //    "categorydetails_before_subcategories","categorydetails_bottom","categorydetails_top",
        //      "manufacturerdetails_after_featured_products","manufacturerdetails_before_featured_products",
        //    "manufacturerdetails_before_filters","manufacturerdetails_before_product_list","manufacturerdetails_bottom",
        //    "manufacturerdetails_top"
        //};

        public static string[] ProductWidgetZones = {
          "productbox_addinfo_after","productbox_addinfo_before", "productbox_addinfo_middle","productbreadcrumb_after",
        "productbreadcrumb_before","productdetails_add_info","productdetails_after_breadcrumb","productdetails_after_pictures",
        "productdetails_before_collateral","productdetails_before_pictures","productdetails_bottom",
        "productdetails_essential_bottom","productdetails_essential_top", "productdetails_inside_overview_buttons_after",
        "productdetails_inside_overview_buttons_before", "productdetails_overview_bottom","productdetails_overview_top",
        "productdetails_top","productreviews_page_bottom", "productreviews_page_inside_review","productreviews_page_top",
        "productsbytag_before_product_list","productsbytag_bottom", "productsbytag_top", "productsearch_page_advanced",
        "productsearch_page_after_results", "productsearch_page_basic", "productsearch_page_before_results"
    };


        public static string[] CategoryWidgetZones = { "categorydetails_after_breadcrumb","categorydetails_after_featured_products",
        "categorydetails_before_featured_products", "categorydetails_before_filters", "categorydetails_before_product_list",
        "categorydetails_before_subcategories","categorydetails_bottom","categorydetails_top"
    };

        

        public static string[] ManufactureWidgetZones = {
         "manufacturerdetails_after_featured_products","manufacturerdetails_before_featured_products",
        "manufacturerdetails_before_filters","manufacturerdetails_before_product_list","manufacturerdetails_bottom",
        "manufacturerdetails_top"
         };


        enum CatalogPages
        {
            Products = 1,
            Categories = 2,
            Manufacturers = 3 
        }

        public static string[] CustomerRoles =
        {
            "Administrators",
            "Forum Moderators",
            "Registered",
            "Guests",
            "Vendors"
        };


       

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

        public static IList<SelectListItem> GetCustomWidgetZoneSelectList(int catalogPageId)
        {
            var productlist = new List<SelectListItem>();
            var categorylist = new List<SelectListItem>();
            var manufacturelist = new List<SelectListItem>();
            var list = new List<SelectListItem>();

            int id = 1;
            foreach (var item in ProductWidgetZones)
            {
                productlist.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                list.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                id++;
            }
            foreach (var item in CategoryWidgetZones)
            {
                categorylist.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                list.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                id++;
            }
            foreach (var item in ManufactureWidgetZones)
            {
                manufacturelist.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                list.Add(new SelectListItem()
                {
                    Value = (id).ToString(),
                    Text = item
                });
                id++;
            }

            if (catalogPageId == 1)
                return productlist;
            else if (catalogPageId == 2)
                return categorylist;
            else if (catalogPageId == 3)
                return manufacturelist;
            else
                return list;
        }

        public static bool TryGetWidgetZoneId(string widgetZone, out int widgetZoneId)
        {
            widgetZoneId = -1;
            var zones = GetCustomWidgetZoneNameValuesAsync().Result;
            if (zones != null && zones.Any(x => x.WidgetName.Equals(widgetZone)))
            {
                widgetZoneId = zones.FirstOrDefault(x => x.WidgetName.Equals(widgetZone)).WidgetId;
                return true;
            }
            return false;
        }

        public static bool TryGetCatalogPageId(string catalogPage, out int catalogPageId)
        {
            catalogPageId = -1;
            var pages = GetCustomCatalogPageNameValuesAsync().Result;
            if (pages != null && pages.Any(x => x.CatalogPageName.Equals(catalogPage)))
            {
                catalogPageId = pages.FirstOrDefault(x => x.CatalogPageName.Equals(catalogPage)).CatalogPageId;
                return true;
            }
            return false;
        }

        //public static async Task<IList<SelectListItem>> GetCustomProductWidgetZoneSelectListAsync()
        //{
        //    var list = new List<SelectListItem>();
        //    var zones = await GetCustomProductWidgetZoneNameValuesAsync();
        //    if (zones != null && zones.Any())
        //    {
        //        foreach (var item in zones)
        //        {
        //            list.Add(new SelectListItem()
        //            {
        //                Value = item.WidgetId.ToString(),
        //                Text = item.WidgetName
        //            });
        //        }
        //    }
        //    return list;
        //}
        //public static async Task<IList<SelectListItem>> GetCustomCategoryWidgetZoneSelectListAsync()
        //{
        //    var list = new List<SelectListItem>();
        //    var zones = await GetCustomCategoryWidgetZoneNameValuesAsync();
        //    if (zones != null && zones.Any())
        //    {
        //        foreach (var item in zones)
        //        {
        //            list.Add(new SelectListItem()
        //            {
        //                Value = item.WidgetId.ToString(),
        //                Text = item.WidgetName
        //            });
        //        }
        //    }
        //    return list;
        //}
        //public static async Task<IList<SelectListItem>> GetCustomManufactureWidgetZoneSelectListAsync()
        //{
        //    var list = new List<SelectListItem>();
        //    var zones = await GetCustomManufactureWidgetZoneNameValuesAsync();
        //    if (zones != null && zones.Any())
        //    {
        //        foreach (var item in zones)
        //        {
        //            list.Add(new SelectListItem()
        //            {
        //                Value = item.WidgetId.ToString(),
        //                Text = item.WidgetName
        //            });
        //        }
        //    }
        //    return list;
        //}

        public static async Task<List<WidgetZoneModel>> GetCustomWidgetZoneNameValuesAsync()
        {
            var zones = new List<WidgetZoneModel>();
            try
            {       
                int x = 1;
                foreach (var widgets in ProductWidgetZones)
                {
                    zones.Add(new WidgetZoneModel()
                    {
                        WidgetId = x++,
                        WidgetName = widgets
                    });
                }
                foreach (var widgets in CategoryWidgetZones)
                {
                    zones.Add(new WidgetZoneModel()
                    {
                        WidgetId = x++,
                        WidgetName = widgets
                    });
                }
                foreach (var widgets in ManufactureWidgetZones)
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
        //public static async Task<List<WidgetZoneModel>> GetCustomManufactureWidgetZoneNameValuesAsync()
        //{
        //    var zones = new List<WidgetZoneModel>();
        //    try
        //    {
        //        int x = 37;
        //        foreach (var widgets in ManufactureWidgetZones)
        //        {
        //            zones.Add(new WidgetZoneModel()
        //            {
        //                WidgetId = x++,
        //                WidgetName = widgets
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
        //    }
        //    return zones;
        //}
        //public static async Task<List<WidgetZoneModel>> GetCustomProductWidgetZoneNameValuesAsync()
        //{
        //    var zones = new List<WidgetZoneModel>();
        //    try
        //    {
        //        int x = 1;
        //        foreach (var widgets in ProductWidgetZones)
        //        {
        //            zones.Add(new WidgetZoneModel()
        //            {
        //                WidgetId = x++,
        //                WidgetName = widgets
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
        //    }
        //    return zones;
        //}
        //public static async Task<List<WidgetZoneModel>> GetCustomCategoryWidgetZoneNameValuesAsync()
        //{
        //    var zones = new List<WidgetZoneModel>();
        //    try
        //    {
        //        int x = 29;
        //        foreach (var widgets in CategoryWidgetZones)
        //        {
        //            zones.Add(new WidgetZoneModel()
        //            {
        //                WidgetId = x++,
        //                WidgetName = widgets
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
        //    }
        //    return zones;
        //}

        public static string GetCustomWidgetZone(int widgetZoneId)
        {
            var zones = GetCustomWidgetZoneNameValuesAsync().Result;
            if (zones != null && zones.Any(x => x.WidgetId == widgetZoneId))
            {
                return zones.FirstOrDefault(x => x.WidgetId == widgetZoneId).WidgetName;
            }
            return null;
        }

        public static string GetCustomCatalogPage(int catalogPageId)
        {
            var pages = GetCustomCatalogPageNameValuesAsync().Result;
            if (pages != null && pages.Any(x => x.CatalogPageId == catalogPageId))
            {
                return pages.FirstOrDefault(x => x.CatalogPageId == catalogPageId).CatalogPageName;
            }
            return null;
        }

        public static List<string> GetCustomCustomerRole(IList<int> customerRoleId)
        {
            var roles = GetCustomCustomerRoleNameValuesAsync().Result;
            if (roles != null)
            {
                var list = new List<string>();

                foreach (var cRI in customerRoleId)
                {
                    if (roles.Any(x => x.Id == cRI))
                    {
                        list.Add(roles.FirstOrDefault(x => x.Id == cRI).Name);
                    }

                }
                return list;
            }
            return null;
        }

        public static async Task<IList<SelectListItem>> GetCustomCatalogPageSelectListAsync()
        {
            var list = new List<SelectListItem>();
            var pages = await GetCustomCatalogPageNameValuesAsync();
            if (pages != null && pages.Any())
            {
                foreach (var item in pages)
                {
                    list.Add(new SelectListItem()
                    {
                      
                        Value = item.CatalogPageId.ToString(),
                        Text = item.CatalogPageName
                    });
                }
            }
            return list;
        }

        public static async Task<List<CatalogPageModel>> GetCustomCatalogPageNameValuesAsync()
        {
            var models = new List<CatalogPageModel>();
            try
            {
         
                foreach (var pages in Enum.GetValues<CatalogPages>())
                {

                    models.Add(new CatalogPageModel()
                    {
                        CatalogPageId = (int)pages,
                        CatalogPageName = Enum.GetName(pages)
                    });
                }
            }
            catch (Exception ex)
            {
                await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
            }
            return models;
        }

        public static async Task<IList<SelectListItem>> GetCustomCustomerRoleSelectListAsync()
        {
            var list = new List<SelectListItem>();
            var customerRoles = await GetCustomCustomerRoleNameValuesAsync();
            if (customerRoles != null && customerRoles.Any())
            {
                foreach (var item in customerRoles)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name
                    });
                }
            }
            return list;
        }

        public static async Task<List<CustomerRoleModel>> GetCustomCustomerRoleNameValuesAsync()
        {
            var models = new List<CustomerRoleModel>();
            try
            {
                int x = 1;
                foreach (var roles in CustomerRoles)
                {
                    models.Add(new CustomerRoleModel()
                    {
                        Id = x++,
                        Name = roles
                    });
                }
            }
            catch (Exception ex)
            {
                await EngineContext.Current.Resolve<ILogger>().ErrorAsync(ex.Message, ex);
            }
            return models;
        }

        public static IList<int> GetGlobalCustomerRoleIds(string selectedCustomerRoleIds)
        {
            List<string> result = selectedCustomerRoleIds.Split(',').ToList();
            var customerRoleIds = new List<int>();
            foreach (var r in result)
            {
                customerRoleIds.Add(Int32.Parse(r));
            }
            return customerRoleIds;
        }

        public static bool ValidateCustomerByRoleIds(IList<int> customerRoleIds, IList<int> currentCustomerRoleIds)
        {
            foreach(var role in currentCustomerRoleIds)
            {
                if (customerRoleIds.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
