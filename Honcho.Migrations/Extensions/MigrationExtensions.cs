using System;
using FluentMigrator;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Honcho.Migrations.Extensions
{
    public static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax WithPrimaryKeyColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax, string columnName)
        {
            return tableWithColumnSyntax
                .WithColumn(columnName)
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .Identity();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithPrimaryKeyColumnNonIdentity(this ICreateTableWithColumnSyntax tableWithColumnSyntax, string columnName)
        {
            return tableWithColumnSyntax
                .WithColumn(columnName)
                .AsInt32()
                .NotNullable()
                .PrimaryKey();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithBasicAuditColumns(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax
                .WithColumn("CreatedBy").AsGuid().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedBy").AsGuid().Nullable()
                .WithColumn("ModifiedOn").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddBasicAuditColumns(this IAlterTableAddColumnOrAlterColumnOrSchemaSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax
                .AddColumn("CreatedBy").AsGuid().Nullable()
                .AddColumn("CreatedOn").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .AddColumn("ModifiedBy").AsGuid().Nullable()
                .AddColumn("ModifiedOn").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(Int32.MaxValue);
        }

        public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsMaxString(this IAlterTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(Int32.MaxValue);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsText(this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(1073741823);
        }

        public static string IdentityInsertOn(this string tableName)
        {
            return ToggleIdentityInsert(tableName, true);
        }

        public static string IdentityInsertOff(this string tableName)
        {
            return ToggleIdentityInsert(tableName, false);
        }

        private static string ToggleIdentityInsert(string tableName, bool on)
        {
            var toggle = on ? "ON" : "OFF";
            return string.Format("SET IDENTITY_INSERT {0} {1}", tableName, toggle);
        }

        public static string GetNextId(this string tableName, string identityColumnName)
        {
            return string.Format("SELECT MAX({0}) FROM {1}", identityColumnName, tableName);
        }
    }
}
