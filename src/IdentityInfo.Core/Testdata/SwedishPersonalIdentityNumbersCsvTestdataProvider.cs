using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Testdata
{
    public class SwedishPersonalIdentityNumbersCsvTestdataProvider : ISwedishPersonalIdentityNumbersTestdataProvider
    {
        private readonly Lazy<Task<IEnumerable<SwedishPersonalIdentityNumber>>> _numbers;

        public SwedishPersonalIdentityNumbersCsvTestdataProvider(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            _numbers = new Lazy<Task<IEnumerable<SwedishPersonalIdentityNumber>>>(async () => await GetSwedishPersonalIdentityNumbersFromStreamAsync(stream));
        }

        public async Task<IEnumerable<SwedishPersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersAsync()
        {
            return await _numbers.Value;
        }

        private async Task<IEnumerable<SwedishPersonalIdentityNumber>> GetSwedishPersonalIdentityNumbersFromStreamAsync(Stream stream)
        {
            var pins = new List<SwedishPersonalIdentityNumber>(250000);
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var row = await reader.ReadLineAsync();
                    if (SwedishPersonalIdentityNumber.TryParse(row, out var pin))
                    {
                        pins.Add(pin);
                    }
                }
            }

            return pins;
        }
    }
}