using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Util;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Admin : System.Web.UI.Page
    {
         const string BASE_PATH = "/Web/Admin";

        private AdminService adminService;
        private RegistryService registryService = RegistryService.Instance();

        public string ResponseJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.adminService = new AdminService();


            String RawUrl = Request.RawUrl;

            string RequestPath = "";
            string requestId = Request.Headers.Get("requestId");

            WebResponse webResponse = new WebResponse();
            WebRequest webRequest = new WebRequest()
             ;

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

                webRequest.requestId = requestId;

                DebugConsole.Debug(this, "RequestPath: " + RequestPath, "Raw: ", RawUrl);
                switch (RequestPath)
                {
                    case "/Event":

                        webResponse = adminService.getEvent(webRequest, Request, true); 
                        break;

                }

            }

            webResponse.sessionData = registryService.getSessionData(requestId);

            Response.Clear();
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            Response.AddHeader("Access-Control-Allow-Methods", "*");
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(JsonConvert.SerializeObject(webResponse));
            Response.End();
        }
    }
}