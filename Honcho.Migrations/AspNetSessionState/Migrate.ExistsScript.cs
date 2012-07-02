namespace Honcho.Migrations.AspNetSessionState
{
    public partial class Migrate 
    {
        private const string ExistsSessionStateScript =
                @"
                   IF (EXISTS (SELECT name FROM sys.tables WHERE (name = N'ASPStateTempSessions')
                    )) select 1 as Found ELSE select 0 as Found
                    ";
    }
}