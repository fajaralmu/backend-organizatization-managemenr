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

        public WebResponse generateAppId(string requestId)
        {
            WebResponse response = WebResponse.success();

            string RandomChar =  StringUtil.GenerateRandomChar(20);

            bool exist = false;
             
            if (null!=requestId)
            {
                SessionData sessionData = registryService.getSessionData(requestId);

                if(null!= sessionData)
                {
                    RandomChar = requestId;
                    exist = true;

                    if(sessionData.User!= null)
                    {
                        response.loggedIn = true;
                    }
                }
            }

            response.message = RandomChar;

            if (!exist)
            {
                registryService.putSession(RandomChar, new SessionData()
                {
                    message = "session_data",
                    requestDate = DateTime.Now
                });
            }
           
            response.debug = RegistryService.getSessions();
            return response;
        }
    }
}