using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Data.Extensions;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Widgets.MySlider.Data.Builder
{
    public class MySlidersCustomerRoleBuilder : NopEntityBuilder<Domains.MySlidersCustomerRole>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               .WithColumn(nameof(MySlidersCustomerRole.MySlidersId)).AsInt32().ForeignKey<MySliders>();
            table
              .WithColumn(nameof(MySlidersCustomerRole.CustomerRoleId)).AsInt32().ForeignKey<CustomerRole>();
        }
    }
}
