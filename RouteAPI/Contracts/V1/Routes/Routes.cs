namespace RouteAPI.Contracts.V1.Routes
{
    public class Routes
    {
        public const string Version = "v1";

        public const string Base = "/" + Version;

        public static class RouteRoutes
        {
            public const string GetAll = Base + "/routes";

            public const string Get = Base + "/route/{id}";

            public const string Create = Base + "/route";

            public const string Delete = Base + "/route/{id}";

            public const string Update = Base + "/route/{id}";
        }
    }
}
