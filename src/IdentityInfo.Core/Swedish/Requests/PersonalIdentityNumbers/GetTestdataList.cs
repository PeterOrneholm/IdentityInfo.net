using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using IdentityInfo.Core.Swedish.Testdata;
using MediatR;

namespace IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers
{
    public class GetTestdataList
    {
        public class Query : IRequest<Result>
        {
            public int? Offset { get; set; }
            public int? Limit { get; set; } = 100;

            public Range<int> Year { get; set; }
            public Range<int> Month { get; set; }
            public Range<int> Day { get; set; }
            public Gender? Gender { get; set; }
            public Range<DateTime> DateOfBirth { get; set; }
            public Range<int> Age { get; set; }

            public class Range<T> where T : struct
            {
                public T? From { get; set; }
                public T? To { get; set; }
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

            private static void AddRangeQueryStringParams<T>(Dictionary<string, string> queryStringParams, string name, Range<T> range, Func<T, string> serializer) where T : struct
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

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IFlatSwedishPersonalIdentityNumbersTestdataProvider _flatSwedishPersonalIdentityNumbersTestdataProvider;

            public Handler(IFlatSwedishPersonalIdentityNumbersTestdataProvider flatSwedishPersonalIdentityNumbersTestdataProvider)
            {
                _flatSwedishPersonalIdentityNumbersTestdataProvider = flatSwedishPersonalIdentityNumbersTestdataProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var testdata = await _flatSwedishPersonalIdentityNumbersTestdataProvider.GetFlatSwedishPersonalIdentityNumbersAsync();

                var testdataList = testdata.ToList();
                var filteredTestdata = ApplyFilters(request, testdataList.AsEnumerable());
                var filteredTestdataList = filteredTestdata.ToList();
                var paginatedTestData = Paginate(request, filteredTestdataList);
               
                return new Result(paginatedTestData, filteredTestdataList.Count, testdataList.Count);
            }

            private static IEnumerable<FlatSwedishPersonalIdentityNumber> ApplyFilters(Query request, IEnumerable<FlatSwedishPersonalIdentityNumber> filteredTestdata)
            {
                var filteredItems = filteredTestdata;

                filteredItems = FilterRange(filteredItems, request.Year, x => x.Year);
                filteredItems = FilterRange(filteredItems, request.Month, x => x.Month);
                filteredItems = FilterRange(filteredItems, request.Day, x => x.Day);
                filteredItems = FilterRange(filteredItems, request.DateOfBirth, x => x.DateOfBirthHint);

                if (request.Age?.From != null)
                {
                    filteredItems = filteredItems.Where(x => x.AgeHint.HasValue && x.AgeHint >= request.Age.From);
                }

                if (request.Age?.To != null)
                {
                    filteredItems = filteredItems.Where(x => x.AgeHint.HasValue && x.AgeHint <= request.Age.To);
                }

                if (request.Gender != null)
                {
                    filteredItems = filteredItems.Where(x => x.GenderHint == request.Gender);
                }

                return filteredItems;
            }

            private static IEnumerable<FlatSwedishPersonalIdentityNumber> FilterRange<T>(IEnumerable<FlatSwedishPersonalIdentityNumber> items, Query.Range<T> range, Func<FlatSwedishPersonalIdentityNumber, T> valueGetter) where T : struct, IComparable<T>
            {
                var filteredItems = items;

                if (range?.From != null)
                {
                    filteredItems = filteredItems.Where(x => valueGetter(x).CompareTo(range.From.Value) >= 0);
                }

                if (range?.To != null)
                {
                    filteredItems = filteredItems.Where(x => valueGetter(x).CompareTo(range.To.Value) <= 0);
                }

                return filteredItems;
            }

            private IEnumerable<FlatSwedishPersonalIdentityNumber> Paginate(Query request, IEnumerable<FlatSwedishPersonalIdentityNumber> filteredTestdata)
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
            public Result(IEnumerable<FlatSwedishPersonalIdentityNumber> filteredNumbers, int filteredNumbersCount, int totalNumbers)
            {
                FilteredNumbers = filteredNumbers;
                FilteredNumbersCount = filteredNumbersCount;
                TotalNumbers = totalNumbers;
            }

            public IEnumerable<FlatSwedishPersonalIdentityNumber> FilteredNumbers { get; }
            public int FilteredNumbersCount { get; }
            public int TotalNumbers { get; }
        }
    }
}
