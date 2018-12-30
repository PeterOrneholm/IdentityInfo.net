using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers
{
    public class GetTestdataList
    {
        public class Query : IRequest<Result>
        {
            public int? Skip { get; set; }
            public int? Take { get; set; } = 1000;
        }

        public class Handler :
            IRequestHandler<Query, Result>
        {
            private readonly IFlatSwedishPersonalIdentityNumbersTestdataProvider _flatSwedishPersonalIdentityNumbersTestdataProvider;

            public Handler(IFlatSwedishPersonalIdentityNumbersTestdataProvider flatSwedishPersonalIdentityNumbersTestdataProvider)
            {
                _flatSwedishPersonalIdentityNumbersTestdataProvider = flatSwedishPersonalIdentityNumbersTestdataProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var testdata = await _flatSwedishPersonalIdentityNumbersTestdataProvider.GetFlatSwedishPersonalIdentityNumbersAsync();

                if (request.Skip.HasValue)
                {
                    testdata = testdata.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    testdata = testdata.Take(request.Take.Value);
                }

                return new Result(testdata);
            }
        }

        public class Result
        {
            public Result(IEnumerable<FlatSwedishPersonalIdentityNumber> swedishPersonalIdentityNumbers)
            {
                SwedishPersonalIdentityNumbers = swedishPersonalIdentityNumbers;
            }

            public IEnumerable<FlatSwedishPersonalIdentityNumber> SwedishPersonalIdentityNumbers { get; }
        }
    }
}
