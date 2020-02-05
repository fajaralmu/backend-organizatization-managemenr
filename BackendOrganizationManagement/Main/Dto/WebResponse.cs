using BackendOrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class WebResponse 
    {
        public string code { get; set; }
        public string message { get; set; }
        public user user { get; set; }
        public BaseEntity entity { get; set; }
        public List<BaseEntity> entities { get; set; }
        public int totalData { get; set; }

        public static WebResponse failed( )
        {
            return new WebResponse {
                code="01",
                message="failed"
            };
        }
        public static WebResponse failed(string msg, string code="01")
        {
            return new WebResponse
            {
                code = code,
                message = msg
            };
        }
        public static WebResponse success()
        {
            return new WebResponse
            {
                code = "00",
                message = "success"
            };
        }
    }
}