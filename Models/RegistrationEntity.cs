using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CURDEF1.Models
{
    public class RegistrationEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Cources { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<int> City { get; set; }
        public string NativePlace { get; set; }

        public virtual City City1 { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual State State1 { get; set; }
    }
}