using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Web.Areas.Swedish.Models
{
    public static class SwedishCoordinationNumberExtensions
    {
        public static char GetDelimiter(this CoordinationNumber swedishCoordinationNumber)
        {
            return swedishCoordinationNumber.To10DigitString()[6];
        }
    }
}
