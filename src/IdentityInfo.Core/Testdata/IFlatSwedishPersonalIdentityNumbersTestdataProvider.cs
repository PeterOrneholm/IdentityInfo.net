using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityInfo.Core.Testdata
{
    public interface IFlatSwedishPersonalIdentityNumbersTestdataProvider
    {
        Task<IEnumerable<FlatSwedishPersonalIdentityNumber>> GetFlatSwedishPersonalIdentityNumbersAsync();
    }
}
