namespace MovieAdvice.Infrastructure
{
    public static class Global
    {
        public static string Secret { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            Secret = configuration["Secret"];
        }
    }
}
