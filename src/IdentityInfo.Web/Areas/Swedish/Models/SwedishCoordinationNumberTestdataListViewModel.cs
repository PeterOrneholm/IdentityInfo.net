using IdentityInfo.Core.Swedish.Requests.CoordinationNumbers;

namespace IdentityInfo.Web.Areas.Swedish.Models
{
    public class SwedishCoordinationNumberTestdataListViewModel
    {
        public SwedishCoordinationNumberTestdataListViewModel(GetTestdataList.Query query, GetTestdataList.Result result)
        {
            Query = query;
            Result = result;
        }

        public GetTestdataList.Query Query { get; }
        public GetTestdataList.Result Result { get; }
    }
}
