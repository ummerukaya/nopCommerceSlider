using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Data.Builder
{
    public class MySlidersBuilder : NopEntityBuilder<MySliders>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(MySliders.Name)).AsString(400).NotNullable();

        }
    }
}
