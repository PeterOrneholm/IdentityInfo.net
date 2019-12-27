using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public class FlatSwedishPersonalIdentityNumbersTestdataProvider : IFlatSwedishPersonalIdentityNumbersTestdataProvider
    {
        private readonly Lazy<Task<List<FlatSwedishPersonalIdentityNumber>>> _numbers;
        private readonly Lazy<Task<HashSet<FlatSwedishPersonalIdentityNumber>>> _numbersHashSet;

        public FlatSwedishPersonalIdentityNumbersTestdataProvider(ISwedishPersonalIdentityNumbersTestdataProvider swedishPersonalIdentityNumbersTestdataProvider)
        {
            _numbers = new Lazy<Task<List<FlatSwedishPersonalIdentityNumber>>>(async () =>
            {
                var originalNumbers = await swedishPersonalIdentityNumbersTestdataProvider.GetSwedishPersonalIdentityNumbersAsync();
                var flatNumbers = originalNumbers.Select(x => new FlatSwedishPersonalIdentityNumber(x));
                var orderedFlatNumbers = flatNumbers.OrderByDescending(x => x.DateOfBirthHint)
                    .ThenByDescending(x => x.BirthNumber)
                    .ToList();

                return orderedFlatNumbers;
            });

            _numbersHashSet = new Lazy<Task<HashSet<FlatSwedishPersonalIdentityNumber>>>(async () =>
            {
                var numbers = await _numbers.Value;
                return new HashSet<FlatSwedishPersonalIdentityNumber>(numbers);
            });
        }

        public async Task<List<FlatSwedishPersonalIdentityNumber>> GetFlatSwedishPersonalIdentityNumbersAsync()
        {
            return await _numbers.Value;
        }

        public async Task<bool> Contains(FlatSwedishPersonalIdentityNumber swedishPersonalIdentityNumber)
        {
            var numbersHashSet = await _numbersHashSet.Value;
            return numbersHashSet.Contains(swedishPersonalIdentityNumber);
        }
    }
}
