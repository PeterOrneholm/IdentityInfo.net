using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public interface ISwedishPersonalIdentityNumbersTestdataProvider
    {
        Task<List<PersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync();
    }
}
