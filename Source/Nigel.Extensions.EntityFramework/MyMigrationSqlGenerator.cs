using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Nigel.Basic;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Migrations;
using System;
using System.Linq;

namespace Nigel.Extensions.EntityFramework
{
    /// <summary>
    /// Class MySqlMigrationSqlGenerator.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.MySqlMigrationsSqlGenerator" />
    public class MyMigrationSqlGenerator : MySqlMigrationsSqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyMigrationSqlGenerator"/> class.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="migrationsAnnotations">The migrations annotations.</param>
        /// <param name="options">The options.</param>
        public MyMigrationSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations, IMySqlOptions options) : base(dependencies, migrationsAnnotations, options)
        {
        }

        /// <summary>
        /// Generates the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="model">The model.</param>
        /// <param name="builder">The builder.</param>
        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
            if (operation is CreateTableOperation || operation is AlterTableOperation)
                CreateTableComment(operation, model, builder);
            if (operation is AddColumnOperation || operation is AlterColumnOperation)
                CreateColumnComment(operation, model, builder);
        }

        /// <summary>
        /// Creates the column comment.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="model">The model.</param>
        /// <param name="builder">The builder.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void CreateTableComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            string tableName = string.Empty;
            if (operation is AlterTableOperation tableOperation)
            {
                tableName = tableOperation.Name;
            }
            if (operation is CreateTableOperation t)
            {
                var addColumnsOperation = t.Columns;
                tableName = t.Name;
                foreach (var item in addColumnsOperation)
                {
                    CreateColumnComment(item, model, builder);
                }
            }
            if (tableName.IsNoneValue())
                throw new Exception("Create table comment error.");
            var description = DbDescriptionHelper.GetDescription(tableName);
            if (description.IsNotNullAll())
            {
                var sqlHelper = Dependencies.SqlGenerationHelper;
                builder
                    .Append($"ALTER TABLE {sqlHelper.DelimitIdentifier(tableName)} COMMENT '{description}'")
                    .AppendLine(sqlHelper.StatementTerminator).EndCommand();
            }
        }

        /// <summary>
        /// Creates the table comment.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="model">The model.</param>
        /// <param name="builder">The builder.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void CreateColumnComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            string tableName = string.Empty;
            string columnName = string.Empty;
            string columnType = string.Empty;
            if (operation is AlterColumnOperation alterColumnOperation)
            {
                tableName = alterColumnOperation.Table;
                columnName = alterColumnOperation.Name;
                columnType = GetColumnType(alterColumnOperation.Schema,
                    alterColumnOperation.Table, alterColumnOperation.Name, alterColumnOperation, model);
            }

            bool isKey = false;
            if (operation is AddColumnOperation addColumnOperation)
            {
                tableName = addColumnOperation.Table;
                columnName = addColumnOperation.Name;
                var annotations = addColumnOperation.GetAnnotations().ToList();
                isKey = annotations.Any(c => c.Name == "MySql:ValueGenerationStrategy");
                columnType = GetColumnType(addColumnOperation.Schema, addColumnOperation.Table,
                    addColumnOperation.Name, addColumnOperation, model);
            }
            if (columnName.IsNoneValue() || tableName.IsNoneValue() || columnType.IsNoneValue())
                throw new Exception($"Create column comment error.{ columnName }/{tableName}/{columnType}");

            string description = DbDescriptionHelper.GetDescription(tableName, columnName);
            if (description.IsNotNullAll())
            {
                var sqlHelper = Dependencies.SqlGenerationHelper;
                builder.Append($"ALTER TABLE {sqlHelper.DelimitIdentifier(tableName)} MODIFY COLUMN {columnName} {columnType} {(isKey ? "AUTO_INCREMENT" : "")} COMMENT '{description}'")
                    .AppendLine(sqlHelper.StatementTerminator).EndCommand();
            }
        }
    }
}