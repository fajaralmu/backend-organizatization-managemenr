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
using System.Web.Script.Services;
using System.Web.Services;

namespace BackendOrganizationManagement.Web
{
    public partial class Public : System.Web.UI.Page
    {
        private static ApplicationService appService = new ApplicationService();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private WebResponse GenerateAppId(string requestId)
        {

            return appService.generateAppId(requestId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GenerateAppId()
        {
            HttpRequest Request = HttpContext.Current.Request;
            WebRequest webRequest = RestUtil.readRequestBody(Request);


            WebResponse response = appService.generateAppId(webRequest.requestId);
            return (StringUtil.serializeCustomModel(response));
        }
    }
}