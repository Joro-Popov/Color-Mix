using ColorMix.Data;
using Microsoft.AspNetCore.Builder;

namespace ColorMix.Web.MiddlewareExtensions
{
    public static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseDatabaseSeeder(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DatabaseSeeder>();
        }
    }
}
