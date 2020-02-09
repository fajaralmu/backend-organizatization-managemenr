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
        private DivisionService divisionService = new DivisionService();
        private SessionService sessionService = new SessionService();

        internal WebResponse DoLogin(HttpRequest request, WebRequest webRequest)
        {

            user requestUser = webRequest.user;

            if (null == requestUser)
            {
                return WebResponse.failed("Invalid Login");
            }

            user AuthUser = userService.GetUserByUsernameAndPassword(requestUser.username, requestUser.password);

            if (AuthUser != null)
            {
                WebResponse response = WebResponse.success();

                user finalUser = (user)ObjectUtil.CopyObjectIgnore(AuthUser, "posts", "password", "institution");

                response.user = finalUser;
                bool updateSession = sessionService.putUser(webRequest.requestId, finalUser);
                if (updateSession)
                {
                    List<division> divisionList = divisionService.GetByInsitutionId(finalUser.institution_id);

                    List<division> resultList = new List<division>();
                    //avoid circular
                    foreach(division Div in divisionList)
                    {
                        resultList.Add((division)ObjectUtil.CopyObjectIgnore(Div, "institution", "sections"));
                    }

                    response.divisions = resultList;
                    return response;
                }
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

        internal WebResponse GetDivisions(WebRequest webRequest)
        {
            SessionData sessionData = this.sessionService.GetSessionData(webRequest);
            if (null != sessionData && null != sessionData.User)
            {
                WebResponse response = WebResponse.success();

                List<division> divisions = divisionService.GetByInsitutionId(sessionData.User.institution_id);

                if (null == divisions)
                {
                    divisions = new List<division>();
                }
                response.divisions = divisions;

                return response;
            }

            return WebResponse.failed();
        }

        internal WebResponse SetDivision(WebRequest webRequest)
        {
            SessionData sessionData = this.sessionService.GetSessionData(webRequest);
            if (null != sessionData && null != sessionData.User)
            {
                WebResponse response = WebResponse.success();
                try
                {
                    object division = divisionService.GetById(webRequest.divisionId);

                    if (null != division)
                    {
                        response.entity = (division)division;
                        sessionData.Division = (division)division;
                        sessionService.updateSessionData(webRequest.requestId, sessionData);

                        response.sessionData = this.sessionService.GetSessionData(webRequest); 
                        return response;
                    }
                   
                }
                catch (Exception ex)
                {

                }
            }

            return WebResponse.failed();
        }
    }
}