using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Util;
using Microsoft.Win32;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackendOrganizationManagement.Main.Handler
{
    public class RegistryService
    { 

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        private static RegistryService reg;
        static Dictionary<string, string> sessionMap = new Dictionary<string, string>();

        public static string GetValue(string sessionKey)
        {
            if (sessionMap.ContainsKey(sessionKey))
                return sessionMap[sessionKey];
            else return null;
        }

        static void SetValue(string sessionKey, string val)
        {
            if (GetValue(sessionKey) != null)
            {
                sessionMap[sessionKey] = val;
            }
            else {
                sessionMap.Add(sessionKey, val);
            }
        }

        public static RegistryService Instance()
        {
            if (null == reg)
            {
                reg = new RegistryService();
            }
            return reg;
        }

        private RegistryService() { }



        public bool putSession(String sessionVal, SessionData sessionData)
        {
            try
            {
                string jsonString = StringUtil.serializeCustomModel(sessionData);
                SetValue(sessionVal, jsonString);
                return true;
            }
            catch (Exception e)
            {
                DebugConsole.Debug(this, "Error Updating registry", e.Message);
            }
             

            return false;

        }
        public SessionData getSessionData(WebRequest req)
        {
            return this.getSessionData(req.requestId);
        }
        public SessionData getSessionData(string sessionVal)
        {
            try
            {
                string sessionValue = GetValue(sessionVal);
                object deserialized = serializer.Deserialize(sessionValue, typeof(SessionData));
                return (SessionData)deserialized;
            }
            catch (Exception e)
            {
                DebugConsole.Debug(this, "Error getting registry", e.Message);
                
            }
            

            return null;
        }
    }
}