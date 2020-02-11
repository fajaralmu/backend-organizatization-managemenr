using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendOrganizationManagement.Web.Api
{
    
    public class PublicController : ApiController
    {
        

        [HttpPost]
        [ActionName("generateappid")] 
        public string generateappid( )
        {
            return Public.GenerateAppId();
        }

        [HttpGet]
        [ActionName("generateappid")]
        public string Getgenerateappid()
        {
            return Public.GenerateAppId();
        }


    }
}