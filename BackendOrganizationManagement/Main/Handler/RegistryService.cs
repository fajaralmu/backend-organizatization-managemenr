using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Util;
using Microsoft.Win32;
using Newtonsoft.Json;
using OrgWebMvc.Main.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
       

        public static Dictionary<string, string> getSessions()
        { //
            return new Dictionary<string, string>();
        }

      //  static string connectionString = "data source=FAJAR-PC\\SQLEXPRESS;initial catalog=mpi_db;user id=sa;password=fjrmnwwrsqlserver;";
		static string connectionString =  "Server=sql.freeasphost.net\\MSSQL2016;Database=mpimedianet_management;uid=mpimedianet;pwd=dakwahmedia";
          

        public static string GetValue(string sessionKey)
        {
            SqlConnection cnn = null;
            
            object value = null;
            try {
                cnn = new SqlConnection(connectionString);
                cnn.Open();
                SqlCommand command = new SqlCommand("select session_key,session_data from [session] where session_key = @sessionKey", cnn);
                // Add the parameters.
                command.Parameters.Add(new SqlParameter("sessionKey", sessionKey));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // while there is another record present
                    while (reader.Read())
                    {
                        // write the data on to the screen
                        value = reader[1];
                    }
                }
            }catch(Exception e)  {  }
            finally
            {
                if(cnn!=null)
                    cnn.Close();
            }
           
            if (value != null)
                return value.ToString();
            else
                return null;
        }

        static void SetValue(string sessionKey, string val)
        {
            

            SqlConnection cnn = null;  
            try
            {
                cnn = new SqlConnection(connectionString);
                cnn.Open();
                SqlCommand command;
                string sessionData = GetValue(sessionKey);

                if (sessionData == null)
                {
                    command = new SqlCommand("INSERT INTO session (session_key, session_data, created_date)" +
                   "VALUES (@0, @1, CURRENT_TIMESTAMP)", cnn);
                    // Add the parameters.
                    command.Parameters.Add(new SqlParameter("0", sessionKey));
                    command.Parameters.Add(new SqlParameter("1", val));
                    
                }
                else {
                    command = new SqlCommand("UPDATE session set session_data= @0 WHERE session_key = @1", cnn);
                    // Add the parameters.
                    command.Parameters.Add(new SqlParameter("0", val));
                    command.Parameters.Add(new SqlParameter("1", sessionKey));
                    
                }
               

                command.ExecuteNonQuery();
            }
            catch (Exception e) { }
            finally
            {
                if (cnn != null)
                    cnn.Close();
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

