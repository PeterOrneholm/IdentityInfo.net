using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Testdata
{
    public interface ISwedishPersonalIdentityNumbersTestdataProvider
    {
        Task<IEnumerable<SwedishPersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync();
    }
}
