using System.Threading;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.CoordinationNumbers
{
    public class Validate
    {
        public class Query : IRequest<Result>
        {
            public string Number { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ISwedishCoordinationNumbersTestdataProvider _coordinationNumbersTestdataProvider;

            public Handler(ISwedishCoordinationNumbersTestdataProvider coordinationNumbersTestdataProvider)
            {
                _coordinationNumbersTestdataProvider = coordinationNumbersTestdataProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var number = request.Number ?? string.Empty;

                if (SwedishCoordinationNumber.TryParse(number, out var result))
                {
                    var numbers = await _coordinationNumbersTestdataProvider.GetSwedishCoordinationNumbersAsync();
                    var isTestdataNumber = numbers.Contains(result);
                    return Result.Valid(number, isTestdataNumber, result);
                }

                return Result.Invalid(number);
            }
        }

        public class Result
        {
            private Result(string numberInput, bool isValid, bool isTestdataNumber, SwedishCoordinationNumber? number)
            {
                NumberInput = numberInput;
                IsValid = isValid;
                IsTestdataNumber = isTestdataNumber;
                Number = number;
            }

            public static Result Valid(string input, bool isTestdataNumber, SwedishCoordinationNumber number)
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
            public SwedishCoordinationNumber? Number { get; }
        }
    }
}
