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
            public int? Skip { get; set; }
            public int? Take { get; set; } = 1000;

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
                var testdataList = testdata.ToList();

                var filteredTestdata = ApplyFilters(request, testdataList.AsEnumerable());

                if (request.Skip.HasValue)
                {
                    filteredTestdata = filteredTestdata.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    filteredTestdata = filteredTestdata.Take(request.Take.Value);
                }

                var filteredTestdataList = filteredTestdata.ToList();

                return new Result(filteredTestdataList, filteredTestdataList.Count, testdataList.Count);
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
        }

        public class Result
        {
            public Result(IEnumerable<FlatSwedishPersonalIdentityNumber> swedishPersonalIdentityNumbers, int swedishPersonalIdentityNumbersCount, int totalRows)
            {
                SwedishPersonalIdentityNumbers = swedishPersonalIdentityNumbers;
                SwedishPersonalIdentityNumbersCount = swedishPersonalIdentityNumbersCount;
                TotalRows = totalRows;
            }

            public IEnumerable<FlatSwedishPersonalIdentityNumber> SwedishPersonalIdentityNumbers { get; }
            public int SwedishPersonalIdentityNumbersCount { get; }
            public int TotalRows { get; }
        }
    }
}
