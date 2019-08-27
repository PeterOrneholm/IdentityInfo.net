using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using ActiveLogin.Identity.Swedish.TestData;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public class SwedishPersonalIdentityNumbersTestdataProvider : ISwedishPersonalIdentityNumbersTestdataProvider
    {
        public Task<IEnumerable<SwedishPersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync()
        {
            return Task.FromResult(SwedishPersonalIdentityNumberTestData.AllPinsByDateDesc());
        }
    }
}