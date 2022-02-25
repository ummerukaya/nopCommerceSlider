using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Data.Extensions;

namespace Nop.Plugin.Widgets.MySlider.Data.Builder
{
    public class MySliderItemBuilder : NopEntityBuilder<MySliderItem>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(MySliderItem.MySlidersId)).AsInt32().ForeignKey<MySliders>();

        }
    }
}
