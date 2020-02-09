using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Util;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BackendOrganizationManagement.Web
{
    public partial class Admin : System.Web.UI.Page
    {
         const string BASE_PATH = "/Web/Admin.aspx";

        private static AdminService adminService = new AdminService();
        private static RegistryService registryService = RegistryService.Instance();

        public string ResponseJson { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Event()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);


            WebResponse response = adminService.getEvent(webRequest, Request, true);
            response.sessionData = registryService.getSessionData(webRequest);
            return (StringUtil.serializeCustomModel(response));
        }

    }


}