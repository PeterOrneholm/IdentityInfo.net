using IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers;

namespace IdentityInfo.Web.Areas.Swedish.Models
{
    public class SwedishPersonalIdentityNumberTestdataListViewModel
    {
        public SwedishPersonalIdentityNumberTestdataListViewModel(GetTestdataList.Query query, GetTestdataList.Result result)
        {
            Query = query;
            Result = result;
        }

        public GetTestdataList.Query Query { get; }
        public GetTestdataList.Result Result { get; }
    }
}
