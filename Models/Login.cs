//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CURDEF1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public partial class Login
    {
        public int UserID { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage ="This field is Required")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="This field is Required")]
        public string Password { get; set; }

        public string LoginErrorMessage { get; set; }
    }
}
