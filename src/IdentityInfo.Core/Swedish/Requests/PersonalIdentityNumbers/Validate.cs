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
            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var number = request.Number ?? string.Empty;

                if (SwedishPersonalIdentityNumber.TryParse(number, out var result))
                {
                    return Task.FromResult(Result.Valid(number, new FlatSwedishPersonalIdentityNumber(result)));
                }

                return Task.FromResult(Result.Invalid(number));
            }
        }

        public class Result
        {
            private Result(string numberInput, bool isValid, FlatSwedishPersonalIdentityNumber? number)
            {
                NumberInput = numberInput;
                IsValid = isValid;
                Number = number;
            }

            public static Result Valid(string input, FlatSwedishPersonalIdentityNumber number)
            {
                return new Result(input, true, number);
            }

            public static Result Invalid(string input)
            {
                return new Result(input, false, null);
            }

            public string NumberInput { get; }
            public bool IsValid { get; }
            public FlatSwedishPersonalIdentityNumber? Number { get; }
        }
    }
}
