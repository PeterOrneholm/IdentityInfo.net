using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public interface IFlatSwedishPersonalIdentityNumbersTestdataProvider
    {
        Task<List<FlatSwedishPersonalIdentityNumber>> GetFlatSwedishPersonalIdentityNumbersAsync();
        Task<bool> Contains(FlatSwedishPersonalIdentityNumber swedishPersonalIdentityNumber);
    }
}
