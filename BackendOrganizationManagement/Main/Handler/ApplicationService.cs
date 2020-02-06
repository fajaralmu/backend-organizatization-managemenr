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
        private RegistryService registryService = RegistryService.Instance();

        internal WebResponse generateAppId(string requestId)
        {
            WebResponse response = WebResponse.success();

            string RandomChar =  StringUtil.GenerateRandomChar(20);

            bool exist = false;

            if(null!=requestId)
            {
                SessionData sessionData = registryService.getSessionData(requestId);

                if(null!= sessionData)
                {
                    RandomChar = requestId;
                    exist = true;
                }
            }

            response.message = RandomChar;

            if (!exist)
            {
                registryService.putSession(RandomChar, new SessionData()
                {
                    message = "MANTAP",
                    requestDate = DateTime.Now
                });
            }
            

            return response;
        }
    }
}