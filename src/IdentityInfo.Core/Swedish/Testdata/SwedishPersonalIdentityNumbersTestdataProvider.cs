using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using ActiveLogin.Identity.Swedish.TestData;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public class SwedishPersonalIdentityNumbersTestdataProvider : ISwedishPersonalIdentityNumbersTestdataProvider
    {
        private readonly List<PersonalIdentityNumber> _allNumbers = PersonalIdentityNumberTestData.AllPinsByDateDesc().ToList();

        public Task<List<PersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync()
        {
            return Task.FromResult(_allNumbers);
        }
    }
}
