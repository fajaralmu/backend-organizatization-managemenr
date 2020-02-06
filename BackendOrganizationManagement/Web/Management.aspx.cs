using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Util;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Management : System.Web.UI.Page
    {
        const string BASE_PATH = "/Web/Management";

        private EntityService entityService;

        public string ResponseJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.entityService = new EntityService();


            String RawUrl = Request.RawUrl;

            string RequestPath = "";
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

                DebugConsole.Debug(this, "RequestPath: " + RequestPath, "Raw: ",RawUrl);
                switch (RequestPath)
                {
                    case "/Add":

                       webResponse = entityService.addEntity(webRequest, Request, true);
                        break;
                    case "/Update":

                        webResponse = entityService.addEntity(webRequest, Request, false);
                        break;
                    case "/Get":

                        webResponse = entityService.filter(webRequest);
                        break;
                    case "/Delete":

                        webResponse = entityService.delete(webRequest);
                        break;

                }

            }
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(JsonConvert.SerializeObject(webResponse));
            Response.End();
        }

    }
}