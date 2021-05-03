using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.WebAdvert.Web
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be atleast 6 characters long!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
