using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using ActiveLogin.Identity.Swedish.TestData;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public class SwedishPersonalIdentityNumbersTestdataProvider : ISwedishPersonalIdentityNumbersTestdataProvider
    {
        private readonly List<SwedishPersonalIdentityNumber> _allNumbers = SwedishPersonalIdentityNumberTestData.AllPinsByDateDesc().ToList();

        public Task<List<SwedishPersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync()
        {
            return Task.FromResult(_allNumbers);
        }
    }
}
