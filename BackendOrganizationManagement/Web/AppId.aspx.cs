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

namespace BackendOrganizationManagement.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        const String BASE_PATH = "/Web/AppId";
         protected void Page_Load(object sender, EventArgs e)
        {
            
            String RawUrl = Request.RawUrl;

            string RequestPath = "";
            WebResponse webResponse = new WebResponse();
            WebRequest webRequest = new WebRequest();

            if (Request.HttpMethod.Equals("POST"))
            {
                if (Request.ContentType.Equals("application/json"))
                {
                    var jsonString = String.Empty;

                    Request.InputStream.Position = 0;
                    using (var inputStream = new StreamReader(Request.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                    }

                    webRequest = (WebRequest)JsonConvert.DeserializeObject(jsonString, typeof(WebRequest));
                }
                if (BASE_PATH.Equals(RawUrl) == false)
                {
                    RequestPath = RawUrl.Substring(BASE_PATH.Length, RawUrl.Length - BASE_PATH.Length);
                }


                switch (RequestPath)
                {
                    case "/Generate":

                        webResponse = GenerateAppId();
                        break; 

                }

            }
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(JsonConvert.SerializeObject(webResponse));
            Response.End(); 
        }

        private WebResponse GenerateAppId()
        {
            WebResponse response = WebResponse.success();
            NameValueCollection header = new NameValueCollection(); 
            string RandomChar = StringUtil.GenerateRandomChar(20); 
            
            response.message = RandomChar;
            return response;
        }
    }
}