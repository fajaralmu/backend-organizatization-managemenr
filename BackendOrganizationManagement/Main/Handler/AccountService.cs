using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Service;
using BackendOrganizationManagement.Models;
using BackendOrganizationManagement.Main.Util;

namespace BackendOrganizationManagement.Main.Handler
{
    public class AccountService
    {
        private UserService userService = new UserService();
        private SessionService sessionService = new SessionService();

        internal WebResponse DoLogin(HttpRequest request, WebRequest webRequest)
        {
             
            user requestUser = webRequest.user;

            if(null == requestUser)
            {
                return WebResponse.failed("Invalid Login");
            }

            user AuthUser = userService.GetUserByUsernameAndPassword(requestUser.username, requestUser.password);

            if(AuthUser!= null)
            {
                WebResponse response = WebResponse.success();
 
                user finalUser = (user) ObjectUtil.CopyObjectIgnore(AuthUser, "posts","password");

                response.user = finalUser;
                bool updateSession = sessionService.putUser(webRequest.requestId, finalUser);
                if(updateSession)
                    return response;
            }

            return WebResponse.failed();

        }

        internal WebResponse DoLogout(HttpRequest request, WebRequest webRequest)
        {
            if (sessionService.removeUser(webRequest.requestId))
            {
                return WebResponse.success();
            }

            return WebResponse.failed();
        }
    }
}