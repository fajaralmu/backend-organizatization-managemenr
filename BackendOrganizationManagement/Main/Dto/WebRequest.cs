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

        public string entity { get; set; }
        public Filter filter { get; set; }

        //internal
        public string requestId { get; set; }

    }
}