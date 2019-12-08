namespace RestaurantApplication.Api.Common
{
    public static class Constants
    {
        public const string EthraiTenantId = "8ae475e9-b8d1-4c9a-8058-0a0ede2c0551";
        public const string UserInvitationNotificationTemplateId = "a04c8250-3932-4cb1-be87-cfc69af0da2d";
        public const string EmailConfirmationNotificationTemplateId = "86996e2e-914c-4294-b2e4-e4ba6dcfbf97";
        public const string UserInvitationIdPlaceHolder = "_id_";
        public const string UserInvitationUserNamePlaceHolder = "_username_";
        public const string TokenPlaceHolder = "_token_";

        public class EnvironmentVariables
        {
            public const string ConfigConnectionString = "Ethrai_ConfigConnectionString";
        }

        public class ConfigSections
        {
            public const string ConnectionStrings = "ConnectionStrings";
        }

        public class Collections
        {
            public const string TablesCollection = "tables";
        }

        public class Errors
        {
            public const string EmptyPayload = nameof(EmptyPayload);
        }
    }
}
