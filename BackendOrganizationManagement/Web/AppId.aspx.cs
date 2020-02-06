using BackendOrganizationManagement.Main.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Routing;
using Newtonsoft.Json;
using BackendOrganizationManagement.Main.Dto;
using System.IO;
using OrgWebMvc.Main.Util;
using BackendOrganizationManagement.Main.Handler;

namespace BackendOrganizationManagement.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        const String BASE_PATH = "/Web/AppId";
        private ApplicationService appService;

        protected void Page_Load(object sender, EventArgs e)
        {

            String RawUrl = Request.RawUrl;
            appService = new ApplicationService();

            string RequestPath = "";
            string requestId = Request.Headers.Get("requestId");

            WebResponse webResponse = new WebResponse();
            WebRequest webRequest = new WebRequest();

            if (Request.HttpMethod.Equals("POST"))
            {
                if (Request.ContentType.Equals("application/json"))
                {
                    webRequest = RestUtil.readRequestBody(Request);
                }
                if (BASE_PATH.Equals(RawUrl) == false)
                {
                    RequestPath = RawUrl.Substring(BASE_PATH.Length, RawUrl.Length - BASE_PATH.Length);
                }

                DebugConsole.Debug(this, "RequestPath:", RequestPath);
                switch (RequestPath)
                {
                    case "/Generate":

                        webResponse = GenerateAppId(requestId); 
                        break;

                }

            }

            webResponse.sessionData = RegistryService.Instance().getSessionData(requestId);

            Response.AddHeader("Access-Control-Allow-Origin", "*"); 
            Response.AddHeader("Access-Control-Allow-Methods", "*");
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(JsonConvert.SerializeObject(webResponse));
            Response.End();
            DebugConsole.Debug("END APP ID");
        }

        private WebResponse GenerateAppId(string requestId)
        {
           
            return appService.generateAppId(requestId);
        }
    }
}