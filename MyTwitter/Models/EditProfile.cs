using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyTwitter.Models
{
    public class EditProfile
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter user name")]
        [MaxLength(length: 25, ErrorMessage = "Maximum length 25 exceeded")]
        public string userName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length 100 exceeded")]
        public string passWord { get; set; }
    }
}