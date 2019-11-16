namespace EventManagement.ApplicationCore.Models
{
    public static class GenderExtensions
    {
        public static string GetStringValue(this Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "m";

                case Gender.Female:
                    return "f";
            }
            return null;
        }

        public static Gender? FromStringValue(string stringValue)
        {
            if (stringValue == null)
                return null;
            stringValue = stringValue?.ToLowerInvariant();

            switch (stringValue)
            {
                case "m":
                    return Gender.Male;

                case "f":
                    return Gender.Female;
            }
            return null;
        }
    }
}