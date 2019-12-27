using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers
{
    public class Randomize
    {
        public class Query : IRequest<Result>
        {
            public int? Count { get; set; } = 5;
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IFlatSwedishPersonalIdentityNumbersTestdataProvider _flatSwedishPersonalIdentityNumbersTestdataProvider;

            public Handler(IFlatSwedishPersonalIdentityNumbersTestdataProvider flatSwedishPersonalIdentityNumbersTestdataProvider)
            {
                _flatSwedishPersonalIdentityNumbersTestdataProvider = flatSwedishPersonalIdentityNumbersTestdataProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var pins = await _flatSwedishPersonalIdentityNumbersTestdataProvider.GetFlatSwedishPersonalIdentityNumbersAsync();
                var pinsList = pins.ToList();

                var random = new Random();
                var numbers = Enumerable.Range(0, request.Count ?? 5)
                                        .Select(x => pinsList[random.Next(pinsList.Count)])
                                        .ToList();

                return new Result(numbers);
            }
        }

        public class Result
        {
            public Result(List<FlatSwedishPersonalIdentityNumber> numbers)
            {
                Numbers = numbers;
            }

            public List<FlatSwedishPersonalIdentityNumber> Numbers { get; }
            public int Count => Numbers.Count;
        }
    }
}
