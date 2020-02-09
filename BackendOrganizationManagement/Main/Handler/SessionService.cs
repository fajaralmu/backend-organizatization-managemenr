using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Models;
using BackendOrganizationManagement.Main.Dto;

namespace BackendOrganizationManagement.Main.Handler
{
    public class SessionService
    {
        private RegistryService registryService = RegistryService.Instance();

        public bool putUser(string requestId, user finalUser)
        {

            SessionData existingSessionData = registryService.getSessionData(requestId);

            if (existingSessionData != null)
            {
                existingSessionData.User = finalUser;
                /**
                Clear Division if logout
    */
                if (finalUser == null)
                {
                    existingSessionData.Division = null;
                }

                registryService.putSession(requestId, existingSessionData);
                return true;
            }

            return false;
        }

        public Boolean removeUser(string requestId)
        {
            return putUser(requestId, null);
        }

        public SessionData GetSessionData(WebRequest request)
        {

            SessionData existingSessionData = registryService.getSessionData(request.requestId);
            return existingSessionData;
        }

        public bool updateSessionData(string requestId, SessionData session)
        {

            SessionData existingSessionData = registryService.getSessionData(requestId);

            if (existingSessionData != null)
            {
                registryService.putSession(requestId, session);
                return true;
            }

            return false;
        }
    }
}