using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Nigel.Extensions.EntityFramework
{
    /// <summary>
    /// The custom migration builder.
    /// </summary>
    public class CustomMigrationBuilder : MigrationBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMigrationBuilder"/> class.
        /// </summary>
        /// <param name="activeProvider">The active provider.</param>
        public CustomMigrationBuilder(string activeProvider) : base(activeProvider)
        {
            IntList = new List<int>();
        }

        public List<int> IntList { get; set; }

        public override OperationBuilder<AddColumnOperation> AddColumn<T>(string name, string table, string type = null, bool? unicode = null, int? maxLength = null, bool rowVersion = false, string schema = null, bool nullable = false, object defaultValue = null, string defaultValueSql = null, string computedColumnSql = null, bool? fixedLength = null, string comment = null, string collation = null, int? precision = null, int? scale = null, bool? stored = null)
        {
            while (true)
            {
                defaultValue = new Random().Next(0, 1000000000);
                if (!IntList.Contains((int)defaultValue)) break;
            }

            return base.AddColumn<T>(name, table, type, unicode, maxLength, rowVersion, schema, nullable, defaultValue,
                defaultValueSql, computedColumnSql);
        }
    }
}
