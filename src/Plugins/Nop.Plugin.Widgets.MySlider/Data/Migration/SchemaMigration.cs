using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.MySlider.Domains;
using Nop.Plugin.Widgets.MySlider.Domains;

namespace Nop.Plugin.Widgets.MySlider.Data.Migration
{
    [SkipMigrationOnUpdate]
    [NopMigration("2020/08/04 12:24:16:2551776", "NopPluginNopStation.MySliders base schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {
            _migrationManager.BuildTable<MySliders>(Create);
            _migrationManager.BuildTable<MySliderItem>(Create);
            _migrationManager.BuildTable<Domains.MySlidersCustomerRole>(Create);
        }
    }
}
