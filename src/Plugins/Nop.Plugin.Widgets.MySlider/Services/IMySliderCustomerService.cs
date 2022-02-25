using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Areas.Admin.Models;
using Nop.Plugin.Widgets.MySlider.Models;

namespace Nop.Plugin.Widgets.MySlider.Services
{
    public interface IMySliderCustomerService
    {
        Task InsertCustomerRoleBySliderId(int id, IList<int> selectedCustomerRoleIds);
        List<int> GetCustomerRoleBySliderId(int id);
        IList<int> GetCurrentCustomerRoleIds(int currentCustomerId);
        //Task<List<SliderModel>> GetCustomerRoleIds(SliderModel slider);
    }
}
