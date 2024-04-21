using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using ActiveLogin.Identity.Swedish.Extensions;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.CoordinationNumbers
{
    public class GetTestdataList
    {
        public class QueryBase
        {
            public int? Offset { get; set; }
            public int? Limit { get; set; } 

            public Range<int>? Year { get; set; }
            public Range<int>? Month { get; set; }
            public Range<int>? Day { get; set; }
            public Gender? Gender { get; set; }
            public Range<DateTime>? DateOfBirth { get; set; }
            public Range<int>? Age { get; set; }
        }

        public class Query : QueryBase, IRequest<Result>
        {
            public Query()
            {
                Limit = 100;
            }

            public string ToQueryString()
            {
                return ToQueryString(this.Offset);
            }

            public string ToQueryString(int? offset)
            {
                var queryStringParams = new Dictionary<string, string>();
                AddRangeQueryStringParams(queryStringParams, nameof(Year), Year, i => i.ToString("D"));
                AddRangeQueryStringParams(queryStringParams, nameof(Month), Month, i => i.ToString("D"));
                AddRangeQueryStringParams(queryStringParams, nameof(Day), Day, i => i.ToString("D"));

                if (Gender.HasValue)
                {
                    queryStringParams.Add(nameof(Gender).ToLower(), Gender.Value.ToString().ToLower());
                }

                AddRangeQueryStringParams(queryStringParams, nameof(DateOfBirth), DateOfBirth, i => i.ToShortDateString());
                AddRangeQueryStringParams(queryStringParams, nameof(Age), Age, i => i.ToString("D"));


                if (offset.HasValue)
                {
                    queryStringParams.Add(nameof(Offset).ToLower(), offset.Value.ToString("D"));
                }

                if (Limit.HasValue)
                {
                    queryStringParams.Add(nameof(Limit).ToLower(), Limit.Value.ToString("D"));
                }

                return GetQueryString(queryStringParams);
            }

            private static void AddRangeQueryStringParams<T>(Dictionary<string, string> queryStringParams, string name, Range<T>? range, Func<T, string> serializer) where T : struct
            {
                if (range == null)
                {
                    return;
                }

                if (range.From.HasValue)
                {
                    queryStringParams.Add($"{name.ToLower()}.from", serializer(range.From.Value));
                }

                if (range.To.HasValue)
                {
                    queryStringParams.Add($"{name.ToLower()}.to", serializer(range.To.Value));
                }
            }

            private static string GetQueryString(Dictionary<string, string> queryStringParams)
            {
                if (!queryStringParams.Any())
                {
                    return string.Empty;
                }

                var queryString = string.Join("&", queryStringParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
                return $"?{queryString}";
            }
        }

        public class ApiQuery : QueryBase, IRequest<ApiResult>
        {
            public ApiQuery()
            {
                Limit = null;
            }
        }

        public class Handler
            : IRequestHandler<Query, Result>,
              IRequestHandler<ApiQuery, ApiResult>
        {
            private readonly ISwedishCoordinationNumbersTestdataProvider _coordinationNumbersTestdataProvider;

            public Handler(ISwedishCoordinationNumbersTestdataProvider coordinationNumbersTestdataProvider)
            {
                _coordinationNumbersTestdataProvider = coordinationNumbersTestdataProvider;
            }

            private async Task<Result> Handle(QueryBase request, CancellationToken cancellationToken)
            {
                var testdata = await _coordinationNumbersTestdataProvider.GetSwedishCoordinationNumbersAsync();
                var filteredTestdata = ApplyFilters(request, testdata);
                var filteredTestdataList = filteredTestdata.ToList();
                var paginatedTestData = Paginate(request, filteredTestdataList);

                var totalAgeRange = new Range<int>
                {
                    From = testdata.Min(x => x.GetAgeHint()),
                    To = testdata.Max(x => x.GetAgeHint())
                };

                var totalDateOfBirthRange = new Range<DateTime>
                {
                    From = testdata.Min(x => x.GetDateOfBirthHint()),
                    To = testdata.Max(x => x.GetDateOfBirthHint())
                };

                return new Result(paginatedTestData, filteredTestdataList.Count, testdata.Count, totalAgeRange, totalDateOfBirthRange);
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                return await Handle((QueryBase)request, cancellationToken);
            }

            public async Task<ApiResult> Handle(ApiQuery request, CancellationToken cancellationToken)
            {
                var result = await Handle((QueryBase)request, cancellationToken);
                var apiResult = new ApiResult(result.FilteredNumbersCount, request.Offset, request.Limit, result.FilteredNumbers);
                return apiResult;
            }

            private static IEnumerable<CoordinationNumber> ApplyFilters(QueryBase request, IEnumerable<CoordinationNumber> filteredTestdata)
            {
                var filteredItems = filteredTestdata;

                filteredItems = FilterRange(filteredItems, request.Year, x => x.Year);
                filteredItems = FilterRange(filteredItems, request.Month, x => x.Month);
                filteredItems = FilterRange(filteredItems, request.Day, x => x.RealDay);
                filteredItems = FilterRange(filteredItems, request.DateOfBirth, x => x.GetDateOfBirthHint());

                if (request.Age?.From != null)
                {
                    filteredItems = filteredItems.Where(x => x.GetAgeHint() >= request.Age.From);
                }

                if (request.Age?.To != null)
                {
                    filteredItems = filteredItems.Where(x => x.GetAgeHint() <= request.Age.To);
                }

                if (request.Gender != null)
                {
                    filteredItems = filteredItems.Where(x => x.GetGenderHint() == request.Gender);
                }

                return filteredItems;
            }

            private static IEnumerable<CoordinationNumber> FilterRange<T>(IEnumerable<CoordinationNumber> items, Range<T>? range, Func<CoordinationNumber, T?> valueGetter) where T : struct, IComparable<T>
            {
                var filteredItems = items;

                if (range?.From != null)
                {
                    filteredItems = filteredItems.Where(x => valueGetter(x)?.CompareTo(range.From.Value) >= 0);
                }

                if (range?.To != null)
                {
                    filteredItems = filteredItems.Where(x => valueGetter(x)?.CompareTo(range.To.Value) <= 0);
                }

                return filteredItems;
            }

            private IEnumerable<CoordinationNumber> Paginate(QueryBase request, IEnumerable<CoordinationNumber> filteredTestdata)
            {
                if (request.Offset.HasValue)
                {
                    filteredTestdata = filteredTestdata.Skip(request.Offset.Value);
                }

                if (request.Limit.HasValue)
                {
                    filteredTestdata = filteredTestdata.Take(request.Limit.Value);
                }

                return filteredTestdata;
            }
        }

        public class Result
        {
            public Result(IEnumerable<CoordinationNumber> filteredNumbers, int filteredNumbersCount, int totalNumbers, Range<int> totalAgeRange, Range<DateTime> totalDateOfBirthRange)
            {
                FilteredNumbers = filteredNumbers;
                FilteredNumbersCount = filteredNumbersCount;
                TotalNumbers = totalNumbers;
                TotalAgeRange = totalAgeRange;
                TotalDateOfBirthRange = totalDateOfBirthRange;
            }

            public IEnumerable<CoordinationNumber> FilteredNumbers { get; }
            public int FilteredNumbersCount { get; }
            public int TotalNumbers { get; }
            public Range<int> TotalAgeRange { get; }
            public Range<DateTime> TotalDateOfBirthRange { get; }
        }

        public class ApiResult
        {
            public ApiResult(int resultCount, int? offset, int? limit, IEnumerable<CoordinationNumber> results)
            {
                ResultCount = resultCount;
                Offset = offset;
                Limit = limit;
                Results = results;
            }

            public int ResultCount { get; }
            public int? Offset { get; }
            public int? Limit { get; }
            public IEnumerable<CoordinationNumber> Results { get; }
        }

        public class Range<T> where T : struct
        {
            public T? From { get; set; }
            public T? To { get; set; }
        }
    }
}
