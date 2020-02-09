using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Service;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Account : System.Web.UI.Page
    { 
        private static AccountService accountService = new AccountService();  
        public string ResponseJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {  
        } 
        

        private WebResponse DoLogout(WebRequest webRequest)
        {
            return accountService.DoLogout(Request, webRequest);
        }
         

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Login()
        {
            HttpRequest Request =   HttpContext.Current.Request; 
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = accountService.DoLogin(Request, webRequest);
             
            return(StringUtil.serializeCustomModel(response));
            
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Logout()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = accountService.DoLogout(Request, webRequest);
             
            return(StringUtil.serializeCustomModel(response)); 
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Divisions()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = accountService.GetDivisions(webRequest);
             
            return(StringUtil.serializeCustomModel(response));
        }

        [WebMethod] 
        [ScriptMethod(ResponseFormat=ResponseFormat.Json)]
        public static string SetDivision()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);

            WebResponse response = accountService.SetDivision(webRequest);
             
            return(StringUtil.serializeCustomModel(response));
        }
    }
}