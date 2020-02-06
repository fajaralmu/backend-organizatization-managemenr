using BackendOrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class SessionData
    {
        public user User { get; set; }
        public division Division { get; set; }
        public string message { get; set; }
        public DateTime requestDate { get; set; }
    }
}