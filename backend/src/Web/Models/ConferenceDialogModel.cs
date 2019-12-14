using Newtonsoft.Json;
using System;

namespace EventManagement.WebApp.Models
{
    public class ConferenceDialogModel
    {
        public string EventName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonIgnore]
        public DateTime? BirthDate { get; set; }

        public int? Age
        {
            get
            {
                if (BirthDate == null)
                    return null;

                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                // Go back to the year the person was born in case of a leap year
                if (BirthDate.Value.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}