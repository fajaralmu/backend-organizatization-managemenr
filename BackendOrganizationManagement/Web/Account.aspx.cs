using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Service;
using BackendOrganizationManagement.Main.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Account : System.Web.UI.Page
    {
         
        const string BASE_PATH = "/Web/Account";

        private AccountService accountService;

        public string ResponseJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            accountService = new AccountService();


            String RawUrl = Request.RawUrl;

            string RequestPath = "";
            WebResponse webResponse = new WebResponse();
            WebRequest webRequest = new WebRequest();

            if (Request.HttpMethod.Equals("POST"))
            {
                if (Request.ContentType.Equals( "application/json"))
                {
                    webRequest = RestUtil.readRequestBody(Request);
                }
                if (BASE_PATH.Equals(RawUrl) == false)
                {
                    RequestPath = RawUrl.Substring(BASE_PATH.Length, RawUrl.Length - BASE_PATH.Length);
                }


                switch (RequestPath)
                {
                    case "/Login":

                        webResponse = DoLogin(webRequest);
                        break;
                    case "/Logout":

                        webResponse = DoLogout(webRequest);
                        break;

                }

            }

            Response.Clear();
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            Response.AddHeader("Access-Control-Allow-Methods", "*");
            Response.ContentType = "application/json; charset=utf-8"; 
            Response.Write(JsonConvert.SerializeObject(webResponse));
            Response.End();
        }

        private WebResponse DoLogout(WebRequest webRequest)
        {
            return accountService.DoLogout(Request, webRequest);
        }

         

        public WebResponse DoLogin(WebRequest WebRequest)
        {

           
            return accountService.DoLogin(Request, WebRequest);
        }
    }
}