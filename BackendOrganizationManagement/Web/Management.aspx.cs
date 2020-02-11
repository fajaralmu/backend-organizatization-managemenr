using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Util;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Management : System.Web.UI.Page
    {

        public static RegistryService registryService = RegistryService.Instance();

        private  static EntityService entityService = new EntityService();
        protected void Page_Load(object sender, EventArgs e)
        {
 

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Add()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = entityService.addEntity(webRequest, Request, true);
            response.sessionData = registryService.getSessionData(webRequest);
            return (StringUtil.serializeCustomModel(response));
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Update()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = entityService.addEntity(webRequest, Request, false);
            response.sessionData = registryService.getSessionData(webRequest);
            return (StringUtil.serializeCustomModel(response));
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Get()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = entityService.filter(webRequest);
            response.sessionData = registryService.getSessionData(webRequest);
            return (StringUtil.serializeCustomModel(response));
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Delete()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = entityService.delete(webRequest);
            response.sessionData = registryService.getSessionData(webRequest);
            return (StringUtil.serializeCustomModel(response));
        }

    }
}