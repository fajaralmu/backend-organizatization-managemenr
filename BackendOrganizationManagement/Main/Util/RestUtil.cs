using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Main.Dto;
using System.IO;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;

namespace BackendOrganizationManagement.Main.Util
{
    public class RestUtil
    {
        internal static WebRequest readRequestBody(HttpRequest Request)
        {
            WebRequest req = new WebRequest();
            req.requestId = Request.Headers.Get("requestId");
            var jsonString = String.Empty;

            Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            DebugConsole.Debug("Request Body: ", jsonString);

            if (jsonString == null || jsonString.Equals(""))
            {
                return req;
            }

            req = (WebRequest)JsonConvert.DeserializeObject(jsonString, typeof(WebRequest));
            //IMPORTANT!!
            req.requestId = Request.Headers.Get("requestId");

            return req;
        }
    }
}