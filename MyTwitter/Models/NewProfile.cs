using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyTwitter.Models
{
    public class NewProfile
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter full name")]
        [MaxLength(length: 30, ErrorMessage = "Maximum length 30 exceeded")]
        public string fullName { get; set; }

        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Please enter email Id")]
        [EmailAddress]
        [MaxLength(length: 50, ErrorMessage = "Maximum length 50 exceeded")]
        public string emailID { get; set; }
        public DateTime Joined { get; set; }
        public bool active { get; set; }
    }
}