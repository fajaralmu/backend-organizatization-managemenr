using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class Column:Attribute
    {
        public bool IgnoreFormField { get; set; }
    }
}