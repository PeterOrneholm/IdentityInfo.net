using IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers;
using IdentityInfo.Core.Swedish.Testdata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace IdentityInfo.Web.Areas.Swedish
{
    public static class SwedishAreaConfiguration
    {
        public const string AreaName = "Swedish";

        public static void AddSwedishAreaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISwedishPersonalIdentityNumbersTestdataProvider, SwedishPersonalIdentityNumbersTestdataProvider>();
            services.AddSingleton<IFlatSwedishPersonalIdentityNumbersTestdataProvider, FlatSwedishPersonalIdentityNumbersTestdataProvider>();

            services.AddSingleton<ISwedishCoordinationNumbersTestdataProvider, SwedishCoordinationNumbersTestdataProvider>();

            services.AddMediatR(typeof(GetTestdataList));
        }
    }
}
