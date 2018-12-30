using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityInfo.Core.Testdata
{
    public class FlatSwedishPersonalIdentityNumbersTestdataProvider : IFlatSwedishPersonalIdentityNumbersTestdataProvider
    {
        private readonly Lazy<Task<IEnumerable<FlatSwedishPersonalIdentityNumber>>> _numbers;

        public FlatSwedishPersonalIdentityNumbersTestdataProvider(ISwedishPersonalIdentityNumbersTestdataProvider swedishPersonalIdentityNumbersTestdataProvider)
        {
            _numbers = new Lazy<Task<IEnumerable<FlatSwedishPersonalIdentityNumber>>>(async () =>
            {
                var originalNumbers = await swedishPersonalIdentityNumbersTestdataProvider.GetSwedishPersonalIdentityNumbersAsync();
                var flatNumbers = originalNumbers.Select(x => new FlatSwedishPersonalIdentityNumber(x));
                var orderedFlatNumbers = flatNumbers.OrderByDescending(x => x.DateOfBirthHint)
                    .ThenByDescending(x => x.BirthNumber);

                return orderedFlatNumbers;
            });
        }

        public async Task<IEnumerable<FlatSwedishPersonalIdentityNumber>> GetFlatSwedishPersonalIdentityNumbersAsync()
        {
            return await _numbers.Value;
        }
    }
}