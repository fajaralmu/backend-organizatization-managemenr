using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class AdditionalFilter:Attribute
    {
        public string join { get; set; }
        public string filter { get; set; }
    }
}