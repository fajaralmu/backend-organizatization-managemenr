using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendOrganizationManagement.Web.Api
{
    public class AdminController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [ActionName("event")]
        public string events()
        {
            return Admin.Event();
        }
		
		[HttpGet]
        [ActionName("event")]
        public string GETevents()
        {
            return Admin.Event();
        }
    }
}