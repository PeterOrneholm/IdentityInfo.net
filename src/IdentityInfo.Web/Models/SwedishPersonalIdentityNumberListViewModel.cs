using System.Collections.Generic;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Web.Models
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