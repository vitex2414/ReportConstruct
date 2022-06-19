using System;
using System.Collections.Generic;

namespace ReportConstruct.Models
{
    public partial class Users
    {
        public string Id { get; set; }
        public string Login { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }

    }
}
