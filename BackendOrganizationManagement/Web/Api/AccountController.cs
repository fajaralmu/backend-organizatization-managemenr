using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendOrganizationManagement.Web.Api
{ 
    public class AccountController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [ActionName( "login")]
        public string login()
        {
            return Account.Login();
        }
        [HttpPost]
        [ActionName("logout")]
        public string logout()
        {
            return Account.Logout();
        }
        [HttpPost]
        [ActionName("divisions")]
        public string divisions()
        {
            return Account.Divisions();
        }
        [HttpPost]
        [ActionName("setdivision")]
        public string setdivision()
        {
            return Account.SetDivision();
        }


    }
}