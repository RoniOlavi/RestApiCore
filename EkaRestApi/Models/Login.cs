using System;
using System.Collections.Generic;

#nullable disable

namespace RestApiCore.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
