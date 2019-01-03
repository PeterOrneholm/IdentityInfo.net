using System.Threading;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers
{
    public class Validate
    {
        public class Query : IRequest<Result>
        {
            public string Number { get; set; } = string.Empty;
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
                var number = request.Number ?? string.Empty;

                if (SwedishPersonalIdentityNumber.TryParse(number, out var result))
                {
                    var flatSwedishPersonalIdentityNumber = new FlatSwedishPersonalIdentityNumber(result);
                    var isTestdataNumber = await _flatSwedishPersonalIdentityNumbersTestdataProvider.Contains(flatSwedishPersonalIdentityNumber);
                    return Result.Valid(number, isTestdataNumber, flatSwedishPersonalIdentityNumber);
                }

                return Result.Invalid(number);
            }
        }

        public class Result
        {
            private Result(string numberInput, bool isValid, bool isTestdataNumber, FlatSwedishPersonalIdentityNumber? number)
            {
                NumberInput = numberInput;
                IsValid = isValid;
                IsTestdataNumber = isTestdataNumber;
                Number = number;
            }

            public static Result Valid(string input, bool isTestdataNumber, FlatSwedishPersonalIdentityNumber number)
            {
                return new Result(input, true, isTestdataNumber, number);
            }

            public static Result Invalid(string input)
            {
                return new Result(input, false, false, null);
            }

            public string NumberInput { get; }
            public bool IsValid { get; }
            public bool IsTestdataNumber { get; }
            public FlatSwedishPersonalIdentityNumber? Number { get; }
        }
    }
}
