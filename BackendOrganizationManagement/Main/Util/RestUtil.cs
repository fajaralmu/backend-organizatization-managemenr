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
            var jsonString = String.Empty;

            Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            DebugConsole.Debug("Request Body: ", jsonString);

            return (WebRequest)JsonConvert.DeserializeObject(jsonString, typeof(WebRequest));
        }
    }
}