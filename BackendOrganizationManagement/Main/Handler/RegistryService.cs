using BackendOrganizationManagement.Main.Dto;
using Microsoft.Win32;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Handler
{
    public class RegistryService
    {
        RegistryKey key;

        private static RegistryService reg;

        public static RegistryService Instance()
        {
            if (null == reg)
            {
                reg = new RegistryService();
            }
            return reg;
        }

        private RegistryService()
        {

        }

        private RegistryKey GetKey()
        {
            RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Software\\Wow6432Node\\MPI_WEB_DATA");
            return key;
        }

        public bool putSession(String sessionVal, SessionData sessionData)
        {
            try {
                key = GetKey();
                string jsonString = JsonConvert.SerializeObject(sessionData);
                key.SetValue(sessionVal, jsonString);
                return true;
            }catch(Exception e)
            {
                DebugConsole.Debug(this, "Error Updating registry", e.Message);
            }
            finally
            {
              if(key!= null)  key.Close();
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
                key = GetKey();
                object regVal = key.GetValue(sessionVal);
                Type regValType = regVal.GetType();
                object deserialized = JsonConvert.DeserializeObject((string)regVal , typeof(SessionData));
                return (SessionData) deserialized;
            }
            catch (Exception e)
            {
                DebugConsole.Debug(this, "Error getting registry", e.Message);
            }
            finally
            {
                if (key != null) key.Close();
            }

            return null;
        }
    }
}