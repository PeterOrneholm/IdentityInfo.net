using System.Collections.Generic;
using IdentityInfo.Core.Testdata;

namespace IdentityInfo.Web.Areas.Swedish.Models
{
    public class SwedishPersonalIdentityNumberListViewModel
    {
        public SwedishPersonalIdentityNumberListViewModel(IEnumerable<FlatSwedishPersonalIdentityNumber> flatNumbers)
        {
            FlatNumbers = flatNumbers;
        }

        public IEnumerable<FlatSwedishPersonalIdentityNumber> FlatNumbers { get; }
    }
}