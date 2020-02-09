using BackendOrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class WebRequest
    {
        public user user { get; set; }
        public division division { get; set; }
        public @event Event { get; set; }
        public institution institution { get; set; }
        public position position { get; set; }
        public post post { get; set; }
        public program program { get; set; }
        public section section { get; set; }
        public member member { get; set; }
        public int divisionId { get; set; }

        public string entity { get; set; }
        public Filter filter { get; set; }

        public int year { get; set; }
        public int month { get; set; }

        //public
        public string requestId { get; set; }

    }
}