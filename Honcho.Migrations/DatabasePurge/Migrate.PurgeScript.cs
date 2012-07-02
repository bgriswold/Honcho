namespace Honcho.Migrations.DatabasePurge
{
    public partial class Migrate 
    {
        private const string PurgeProceduresScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- procedures
            select @stmt = isnull( @stmt + @n, '' ) +
                --'drop procedure [' + name + ']'
                'drop procedure [' + schema_name(schema_id) + '].[' + name + ']'
            from sys.procedures

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeConstraintsScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- check constraints
            select @stmt = isnull( @stmt + @n, '' ) +
                'alter table [' + schema_name(schema_id) + '].[' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
            from sys.check_constraints

            print @stmt
            exec sp_executesql @stmt";

 
        private const string PurgeFunctionsScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- functions
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop function [' + schema_name(schema_id) + '].[' + name + ']'
            from sys.objects
            where type in ( 'FN', 'IF', 'TF' )

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeViewsScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- views
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop view [' + schema_name(schema_id) + '].[' + name + ']'
            from sys.views

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeForeignKeysScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- foreign keys
            select @stmt = isnull( @stmt + @n, '' ) +
                'alter table [' + schema_name(schema_id) + '].[' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
            from sys.foreign_keys

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeTablesScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- tables
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop table [' + schema_name(schema_id) + '].[' + name + ']'
            from sys.tables

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeUdtScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- user defined types
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop type [' + schema_name(schema_id) + '].[' + name + ']'
            from sys.types
            where is_user_defined = 1

            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeSchemasScript =
            @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- schemas
            select @stmt = isnull( @stmt + @n, '' ) +
                'drop schema [' + name + ']'
            from sys.schemas 
            where name not in ('dbo', 'guest', 'INFORMATION_SCHEMA', 'sys')
	            and name not like ('db_%')
	
            print @stmt
            exec sp_executesql @stmt";

        private const string PurgeRolesScript =
              @"Use $DATABASE_NAME$;
            declare @n char(1)
            set @n = char(10)

            declare @stmt as nvarchar(max)

            -- roles
            select @stmt = isnull( @stmt + @n, '' ) +
                'EXEC sp_droprole [' + name + ']'
            from sysusers 
            where issqlrole = 1 AND name like ('aspnet_%')
            print @stmt
            exec sp_executesql @stmt";
    }
}