﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.WebAdvert.Web
{
    public class ConfirmModel
    {

        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
