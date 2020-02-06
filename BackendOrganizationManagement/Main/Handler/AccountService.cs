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

                response.user = (user) ObjectUtil.CopyObjectIgnore(AuthUser, "posts","password");
                return response;
            }

            return WebResponse.failed();

        }

        internal WebResponse DoLogout(HttpRequest request, WebRequest webRequest)
        {
            return WebResponse.success();
        }
    }
}