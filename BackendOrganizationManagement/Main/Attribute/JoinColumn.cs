using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class JoinColumn:Attribute
    {
        public string Name { get; set; }
        public string Converter { get; set; }
    }
}