using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.CoordinationNumbers
{
    public class Randomize
    {
        public class Query : IRequest<Result>
        {
            public int? Count { get; set; } = 5;
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ISwedishCoordinationNumbersTestdataProvider _coordinationNumbersTestdata;

            public Handler(ISwedishCoordinationNumbersTestdataProvider coordinationNumbersTestdata)
            {
                _coordinationNumbersTestdata = coordinationNumbersTestdata;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var pins = await _coordinationNumbersTestdata.GetSwedishCoordinationNumbersAsync();
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
            public Result(List<SwedishCoordinationNumber> numbers)
            {
                Numbers = numbers;
            }

            public List<SwedishCoordinationNumber> Numbers { get; }
            public int Count => Numbers.Count;
        }
    }
}
