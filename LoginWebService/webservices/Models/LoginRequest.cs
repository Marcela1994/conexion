using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webservices.Models
{
    public class LoginRequest
    {
        public string nombreUsuario { get; set; }

        public string claveUsuario { get; set; }
    }
}