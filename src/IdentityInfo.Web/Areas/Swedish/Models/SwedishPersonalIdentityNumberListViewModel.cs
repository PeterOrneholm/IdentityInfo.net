using System.Collections.Generic;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Web.Areas.Swedish.Models
{
    public class SwedishPersonalIdentityNumberListViewModel
    {
        public SwedishPersonalIdentityNumberListViewModel(IEnumerable<SwedishPersonalIdentityNumber> numbers)
        {
            Numbers = numbers;
        }

        public IEnumerable<SwedishPersonalIdentityNumber> Numbers { get; }
    }
}