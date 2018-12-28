using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Testdata
{
    public static class SwedishPersonalIdentityNumbersTestdataParser
    {
        public static async Task<IEnumerable<SwedishPersonalIdentityNumber>> ParseCsvAsync(Stream stream)
        {
            var pins = new List<SwedishPersonalIdentityNumber>(250000);
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var row = await reader.ReadLineAsync();
                    if(SwedishPersonalIdentityNumber.TryParse(row, out var pin))
                    {
                        pins.Add(pin);
                    }
                }
            }

            return pins;
        }
    }
}
