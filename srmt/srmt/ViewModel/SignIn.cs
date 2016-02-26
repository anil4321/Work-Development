using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace srmt.ViewModel
{
    public class SignIn
    {
        public string uid { get; set; }
        [Display(Name = "Welcome!")]
        public string name { get; set; }
    }
}