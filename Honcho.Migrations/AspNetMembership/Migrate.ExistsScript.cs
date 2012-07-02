namespace Honcho.Migrations.AspNetMembership
{
    public partial class Migrate 
    {
        private const string ExistsMembershipScript =
                @"
                    IF (EXISTS (SELECT name FROM sys.tables WHERE (name = N'aspnet_Membership')
                    )) select 1 as Found ELSE select 0 as Found                  
                    ";
    }
}