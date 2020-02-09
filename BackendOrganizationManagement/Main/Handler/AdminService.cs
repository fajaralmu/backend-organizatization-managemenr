using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Service;
using BackendOrganizationManagement.Models;

namespace BackendOrganizationManagement.Main.Handler
{
    public class AdminService
    {

        private RegistryService registryService =  RegistryService.Instance();
        private EventService eventService = new EventService();

        public WebResponse getEvent(WebRequest webRequest, HttpRequest request, bool v)
        {
            SessionData sessionData = registryService.getSessionData(webRequest.requestId);
            if (sessionData == null || sessionData.Division == null)
            {
                return WebResponse.failed();
            }

            int divisionId = sessionData.Division.id;

            List<BaseEntity> events = eventService.GetByMonthAndYear(webRequest.month, webRequest.year, divisionId);

            WebResponse response = WebResponse.success();
            response.entities = events;

            return response;
        }
    }
}