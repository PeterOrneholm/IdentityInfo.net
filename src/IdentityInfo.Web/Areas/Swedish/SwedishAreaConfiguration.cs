using System.Reflection;
using IdentityInfo.Core.Testdata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityInfo.Web.Areas.Swedish
{
    public static class SwedishAreaConfiguration
    {
        public const string AreaName = "Swedish";

        public static void AddSwedishAreaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISwedishPersonalIdentityNumbersTestdataProvider>(provider =>
            {
                var assembly = typeof(SwedishAreaConfiguration).GetTypeInfo().Assembly;
                var csvStream = assembly.GetManifestResourceStream("IdentityInfo.Web.Testdata.SwedishPersonalIdentityNumbers_Testdata_181217.csv");
                return new SwedishPersonalIdentityNumbersCsvTestdataProvider(csvStream);
            });

            services.AddSingleton<IFlatSwedishPersonalIdentityNumbersTestdataProvider, FlatSwedishPersonalIdentityNumbersTestdataProvider>();
        }
    }
}