using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public interface ISwedishCoordinationNumbersTestdataProvider
    {
        Task<List<SwedishCoordinationNumber>> GetSwedishCoordinationNumbersAsync();
    }
}
