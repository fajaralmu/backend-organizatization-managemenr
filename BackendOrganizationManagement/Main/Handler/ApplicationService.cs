using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Main.Dto;
using System.Collections.Specialized;
using BackendOrganizationManagement.Main.Util;

namespace BackendOrganizationManagement.Main.Handler
{
    public class ApplicationService
    {
        internal WebResponse generateAppId()
        {
            WebResponse response = WebResponse.success(); 
            string RandomChar = StringUtil.GenerateRandomChar(20);

            response.message = RandomChar;

            return response;
        }
    }
}