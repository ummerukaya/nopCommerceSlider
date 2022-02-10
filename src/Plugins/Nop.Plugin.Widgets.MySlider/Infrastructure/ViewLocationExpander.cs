using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Widgets.MySlider.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.themename";
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {

            if (context.AreaName == "Admin")
            {
                viewLocations = new[] {
                    $"/Plugins/Nop.MySlider/Areas/Admin/Views/Shared/{{0}}.cshtml",
                    $"/Plugins/Nop.MySlider/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
                }.Concat(viewLocations);
            }
            else if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {

                viewLocations = new[] {
                        $"/Plugins/Nop.MySlider/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                        $"/Plugins/Nop.MySlider/Themes/{theme}/Views/{{1}}/{{0}}.cshtml"
                    }.Concat(viewLocations);
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            
        }
    }
}
