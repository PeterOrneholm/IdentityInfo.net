using System;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public struct FlatSwedishPersonalIdentityNumber
    {
        private static int? TryGetAgeHint(SwedishPersonalIdentityNumber pin, DateTime dateOfBirthHint)
        {
            if (dateOfBirthHint > DateTime.Now)
            {
                return null;
            }

            return pin.GetAgeHint();
        }

        public FlatSwedishPersonalIdentityNumber(SwedishPersonalIdentityNumber pin)
        {
            TwelveDigitString = pin.To12DigitString();
            TenDigitString = pin.To10DigitString();
            Year = pin.Year;
            Month = pin.Month;
            Day = pin.Day;
            Delimiter = TenDigitString[6];
            BirthNumber = pin.BirthNumber;
            Checksum = pin.Checksum;
            GenderHint = pin.GetGenderHint();
            DateOfBirthHint = pin.GetDateOfBirthHint();
            AgeHint = TryGetAgeHint(pin, DateOfBirthHint);
        }

        public string TwelveDigitString { get; }
        public string TenDigitString { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public char Delimiter { get; }
        public int BirthNumber { get; }
        public int Checksum { get; }
        public DateTime DateOfBirthHint { get; }
        public Gender GenderHint { get; }
        public int? AgeHint { get; }
    }
}