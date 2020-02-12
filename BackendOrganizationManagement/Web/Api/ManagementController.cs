using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendOrganizationManagement.Web.Api
{
    public class ManagementController : ApiController
    {
        [HttpPost]
        [ActionName("add")]
        public string add()
        {
            return Management.Add();
        }
        [HttpPost]
        [ActionName("update")]
        public string update()
        {
            return Management.Update();
        }
        [HttpPost]
        [ActionName("get")]
        public string get()
        {
            return Management.Get();
        }
        [HttpPost]
        [ActionName("delete")]
        public string delete()
        {
            return Management.Delete();
        }
		
		/** ================GET=============== **/
		[HttpGet]
        [ActionName("add")]
        public string Getadd()
        {
            return Management.Add();
        }
        [HttpGet]
        [ActionName("update")]
        public string Getupdate()
        {
            return Management.Update();
        }
        [HttpGet]
        [ActionName("get")]
        public string Getget()
        {
            return Management.Get();
        }
        [HttpGet]
        [ActionName("delete")]
        public string Getdelete()
        {
            return Management.Delete();
        }
    }
}