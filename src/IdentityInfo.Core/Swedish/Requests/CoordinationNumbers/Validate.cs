using System;
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

                try
                {
                    var result = CoordinationNumber.Parse(number);
                    var numbers = await _coordinationNumbersTestdataProvider.GetSwedishCoordinationNumbersAsync();
                    var isTestdataNumber = numbers.Contains(result);

                    return Result.Valid(number, isTestdataNumber, result);
                }
                catch (Exception e)
                {
                    return Result.Invalid(number, e.Message);
                }
            }
        }

        public class Result
        {
            private Result(string numberInput, bool isValid, string invalidReason, bool isTestdataNumber, CoordinationNumber? number)
            {
                NumberInput = numberInput;
                IsValid = isValid;
                InvalidReason = invalidReason;
                IsTestdataNumber = isTestdataNumber;
                Number = number;
            }

            public static Result Valid(string input, bool isTestdataNumber, CoordinationNumber number)
            {
                return new Result(input, true, string.Empty, isTestdataNumber, number);
            }

            public static Result Invalid(string input, string invalidReason)
            {
                return new Result(input, false, invalidReason, false, null);
            }

            public string NumberInput { get; }
            public bool IsValid { get; }
            public string InvalidReason { get; }
            public bool IsTestdataNumber { get; }
            public CoordinationNumber? Number { get; }
        }
    }
}
