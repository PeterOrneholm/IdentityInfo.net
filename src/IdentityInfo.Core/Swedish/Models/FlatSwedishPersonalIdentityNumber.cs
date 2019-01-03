using System;
using System.Collections.Generic;
using ActiveLogin.Identity.Swedish;

namespace IdentityInfo.Core.Swedish.Testdata
{
    public struct FlatSwedishPersonalIdentityNumber : IEquatable<FlatSwedishPersonalIdentityNumber>
    {
        private static int? TryGetAgeHint(SwedishPersonalIdentityNumber pin, DateTime dateOfBirthHint)
        {
            if (dateOfBirthHint > DateTime.UtcNow.Date)
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


        public override bool Equals(object obj)
        {
            return obj is FlatSwedishPersonalIdentityNumber && Equals((FlatSwedishPersonalIdentityNumber)obj);
        }

        public bool Equals(FlatSwedishPersonalIdentityNumber other)
        {
            return TwelveDigitString == other.TwelveDigitString;
        }

        public override int GetHashCode()
        {
            return TwelveDigitString.GetHashCode();
        }

        public static bool operator ==(FlatSwedishPersonalIdentityNumber number1, FlatSwedishPersonalIdentityNumber number2)
        {
            return number1.Equals(number2);
        }

        public static bool operator !=(FlatSwedishPersonalIdentityNumber number1, FlatSwedishPersonalIdentityNumber number2)
        {
            return !(number1 == number2);
        }
    }
}