using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class EndUser
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a Valid Email")]
        [Required(ErrorMessage = "Please Enter an Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter a Valid Password")]
        [MinLength(6, ErrorMessage = "Your Password needs to be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // [Required(ErrorMessage = "Please Enter a Username")]
        // [MinLength(4, ErrorMessage = "Your Username needs to be at least 4 characters long")]
        public string Username { get; set; }
    }
}