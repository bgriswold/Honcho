namespace Honcho.Migrations
{
    public static class DbSchema
    {
        public static class Schemas
        {
            public static readonly string Dbo = "dbo";
        }

        public static class Tables
        {
            public static readonly string User = "User";
           
        }

        public static class PrimaryKeys
        {
            public static readonly string User = "UserId";
           
        }

        public static class ForeignKeys
        {
            public static readonly string UserAnswerToUser = "FK_UserAnswer_UserId_User_UserId";
        }
    }
}
