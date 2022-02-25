using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Domains
{
    public class MySlidersCustomerRole : BaseEntity
    {
        public int MySlidersId { get; set; }
        public int CustomerRoleId { get; set; }
    }
}
