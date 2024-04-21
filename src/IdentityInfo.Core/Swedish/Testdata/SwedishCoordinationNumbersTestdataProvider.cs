using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using ActiveLogin.Identity.Swedish.TestData;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public class SwedishCoordinationNumbersTestdataProvider : ISwedishCoordinationNumbersTestdataProvider
    {
        private readonly List<CoordinationNumber> _allNumbers = CoordinationNumberTestData.AllCoordinationNumbersByDateDesc().ToList();

        public Task<List<CoordinationNumber>> GetSwedishCoordinationNumbersAsync()
        {
            return Task.FromResult(_allNumbers);
        }
    }
}
