using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Widgets.MySlider.Factories;
using Nop.Plugin.Widgets.MySlider.Services;

namespace Nop.Plugin.Widgets.MySlider.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<IMySliderService, MySliderService>();
            services.AddScoped<IMySliderCustomerService, MySliderCustomerService>();
            services.AddScoped<IMySliderModelFactory, MySliderModelFactory>();
        }
    }
}
