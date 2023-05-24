namespace AdvancedSystems.Backend.Contracts;

public static class ApiRoutes
{
    public const string Root = "api";

    public const string Version = "v1";

    public const string Base = $"{Root}/{Version}";

    public static class BookVersion1
    {
        public const string Create = $"{Base}/book";

        public const string GetAll = $"{Base}/book";

        public const string Get = $"{Base}/book/{{id}}";

        public const string Update = $"{Base}/book/{{id}}";

        public const string Delete = $"{Base}/book/{{id}}";
    }
}
