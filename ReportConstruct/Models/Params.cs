﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ReportConstruct.Models
{
    public partial class Params
    {
        public string Id { get; set; }
        public string ParamName { get; set; }
        public string QueryId { get; set; }
        public string ParamValue { get; set; }

        public virtual Queries Query { get; set; }
    }
}