using BackendOrganizationManagement.Main.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Collections;

namespace BackendOrganizationManagement.Main.Util
{
    public class StringUtil
    {
        private const string Numbers = "1234567890";
        private const string Chars = "qwertyuiopasdfghjklzxcvbnm1234567890";

        public static string DateTimeToString(DateTime Date)
        {
            return Date.Year + "-" + AddZero(Date.Month, 10) + "-" + AddZero(Date.Day, 10) + " " + AddZero(Date.Hour, 10) + ":" + AddZero(Date.Minute, 10) + ":" + AddZero(Date.Second, 10) + "." + Date.Millisecond;
        }

        public static string DateAcceptableForHtmlInput(DateTime Date)
        {
            return Date.Year + "-" + AddZero(Date.Month, 10) + "-" + AddZero(Date.Day, 10);
        }

        public static string AddZero(int Val, int Min)
        {
            string N = Val.ToString();
            string MinStr = Min.ToString();

            int ValLength = N.Length;
            int MinLength = MinStr.Length;
            int Diff = MinLength - ValLength;
            for (int i = 1; i <= Diff; i++)
            {
                N = "0" + N;
            }
            return N;
        }

        private static bool IsItemAlreadyExist(List<string> list, string val)
        {
            foreach (string s in list)
            {
                if (s.Equals(val))
                {
                    return true;
                }
            }
            return false;
        }

        private static string ReplaceParamValue(string CurrentQueryString, string param, object value)
        {
            string[] paramAndValue = CurrentQueryString.Split('&');
            string result = "";
            List<string> queryKeys = new List<string>();
            for (int i = 0; i < paramAndValue.Length; i++)
            {
                string itemParamValue = paramAndValue[i];
                string itemParam = itemParamValue.Split('=')[0];
                if (IsItemAlreadyExist(queryKeys, itemParam))
                {
                    continue;
                }
                queryKeys.Add(itemParam);
                string itemValue = itemParamValue.Split('=')[1];
                if (itemParam.Equals(param))
                {
                    itemValue = (string)value;
                }
                result += itemParam + "=" + itemValue;
                if (i < paramAndValue.Length - 1)
                {
                    result += "&";
                }


            }
            return result;
        }

        public static string AddURLParam(string CurrentURL, string param, object value)
        {
            if (CurrentURL.Contains("?") && CurrentURL.Contains("="))
            {
                if (CurrentURL.Contains(param))
                {
                    string[] urlAndParam = CurrentURL.Split('?');
                    string paramNew = ReplaceParamValue(urlAndParam[1], param, value);
                    return urlAndParam[0] + "?" + paramNew;
                }
                else
                {
                    return CurrentURL + "&" + param + "=" + value;
                }
            }
            else
            {
                return CurrentURL + "?" + param + "=" + value;
            }
        }

        public static String GenerateRandomChar(int Length)
        {
            if (Length == 0)
            {
                return "NULL";
            }

            string RandomString = "";
            int Size = Chars.Length;
            Random R = new Random();
            for (int i = 0; i < Length; i++)
            {

                int Index = R.Next(0, Size);
                RandomString += Chars.ElementAt(Index);
            }
            return RandomString;
        }

        public static String GenerateRandomNumber(int Length)
        {
            if (Length == 0)
            {
                return "NULL";
            }

            string RandomString = "";
            int Size = Numbers.Length;
            Random R = new Random();

            for (int i = 0; i < Length; i++)
            {

                int Index = R.Next(0, Size);
                RandomString += Numbers.ElementAt(Index);
            }

            return RandomString;
        }

        public static string ToUpperCase(int index, string data)
        {
            string result = "";
            if (data == null)
            {
                return "";
            }
            for (int i = 0; i < data.Length; i++)
            {
                char Char = data[i];
                if (i == index)
                {
                    Char = Char.ToUpper(data[i]);
                }
                result += Char;
            }
            return result;
        }

        public static bool NotNullAndNotBlank(object Obj)
        {
            if (Obj != null && !Obj.ToString().Equals(""))
            {
                return true;
            }
            return false;
        }

        public static string AddSortQuery(string orderby, string ordertype)
        {
            string sql = "";
            if (NotNullAndNotBlank(orderby))
            {
                sql += " ORDER BY " + orderby;
                if (NotNullAndNotBlank(ordertype))
                {
                    sql += " " + ordertype;
                }
            }
            return sql;
        }

        public static bool NotNullAndNotBlank(params object[] Objs)
        {
            foreach (object Obj in Objs)
            {
                if (Obj == null || Obj.ToString().Equals(""))
                {
                    return false;
                }
            }
            return true;
        }

        public static string AddDateFilterQuery(Dictionary<string, object> Params, string entity, string dateColumnName, bool beginWithAnd)
        {
            string sql = "";

            string day = Params.ContainsKey(dateColumnName + ".day") ? Params[dateColumnName + ".day"].ToString() : "";
            string month = Params.ContainsKey(dateColumnName + ".month") ? (string)Params[dateColumnName + ".month"].ToString() : "";
            string year = Params.ContainsKey(dateColumnName + ".year") ? (string)Params[dateColumnName + ".year"].ToString() : "";

            string sqlDay = (StringUtil.NotNullAndNotBlank(day) ? "  DAY([" + entity + "].[" + dateColumnName + "]) = " + day : "");

            string sqlMonth = (StringUtil.NotNullAndNotBlank(month) ? "  MONTH([" + entity + "].[" + dateColumnName + "]) = " + month : "");

            string sqlYear = (StringUtil.NotNullAndNotBlank(year) ? "  YEAR([" + entity + "].[" + dateColumnName + "]) = " + year : "");

            sql += sqlDay;
            if (sqlDay != "" && sqlMonth != "")
            {
                sql += " and ";
            }
            sql += sqlMonth;
            if (sqlMonth != "" && sqlYear != "")
            {
                sql += " and " + sqlYear;
            }

            if (sql != "" && beginWithAnd)
            {
                sql = " and " + sql;
            }
            return sql;
        }

        public static bool NotNullAndNotBlankAndTypeOf(object Obj, Type t)
        {
            if (NotNullAndNotBlank(Obj) && Obj.GetType().Equals((t)))
            {
                return true;
            }
            return false;
        }

        public static Dictionary<string, object> QUeryStringToDict(string q)
        {
            Dictionary<string, object> Map = new Dictionary<string, object>();
            try
            {
                string[] Params = q.Split('&');
                foreach (string Param in Params)
                {
                    if (Param != null && Param != "")
                    {
                        string[] Prop = Param.Split('=');
                        Map.Add(Prop[0], Prop[1]);
                    }
                }
                return Map;

            }
            catch (Exception e)
            {
                return new Dictionary<string, object>();
            }
        }

        public static string serializeCustomModel(object Object)
        {
            if (Object == null)
            {
                return null;
            }

            Type objectType = Object.GetType();

            bool customMode = haSCustomModel(objectType);
            bool isList = objectType.Name.Contains("List");
            bool isDictionary = objectType.Name.Contains("Dictionary");

            int i = 0;
            if (isList)
            {
                IList list = (IList)Object;

                if (list.Count == 0) { return "[]"; }

                StringBuilder result = new StringBuilder("[");

                foreach (object Item in list)
                {
                    result = result.Append(serializeCustomModel(Item));
                    i++;
                    if (i < list.Count)
                    {
                        result = result.Append(",");
                    }
                }

                return result.Append("]").ToString();

            }

            if (isDictionary)
            {
                StringBuilder result = new StringBuilder("{");


                IDictionary Map = (IDictionary)Object;

                i = 0;
                foreach(string key in Map.Keys)
                {
                    result = result.Append(doubleQuote(key)).Append(":").Append(serializeCustomModel(Map[key]));
                    i++;
                    if (i < Map.Keys.Count)
                    {
                        result = result.Append(",");
                    }
                }
                return result.Append("}").ToString();
            }

            if (!customMode)
            {
                return doubleQuote(Object);
            }
            PropertyInfo[] propInfos = objectType.GetProperties();

            string preffix = "{";
            string suffix = "}";

            StringBuilder builder = new StringBuilder(preffix);
            i = 0;
            int fieldCount = propInfos.Length;

            foreach (PropertyInfo propInfo in propInfos)
            {
               
                //if ignored = 
                if (propInfo.GetCustomAttribute(typeof(JsonIgnoreAttribute)) != null)
                {
                    fieldCount--;
                    continue;
                }

                string propName = propInfo.Name;

                builder = builder.Append(doubleQuote(propName)).Append(":");

                //value
                object value = propInfo.GetValue(Object);
                if (value != null)
                {

                    string StrValue = serializeCustomModel(value);
                    builder = builder.Append(StrValue);

                }
                else if (value == null)
                {
                    builder = builder.Append("null");
                }
                i++;
                if (i < fieldCount)
                {
                    builder = builder.Append(",");
                }

            }

            if (builder.ToString().EndsWith(","))
            {
                builder = builder.Remove(builder.ToString().Length - 1, 1);
            }
            return builder.Append(suffix).ToString();

        }

        static string doubleQuote(object Object)
        {

            Type type = Object.GetType();

            if (typeOf(type, typeof(bool), typeof(Boolean)))
            {
                bool obj = (bool)Object;
                return "" + (obj ? "true" : "false") + "";
            }
            else if (IsNumericType(Object))
            {
                return Object.ToString();
            }
            string quot  = "\"";
            if(Object!=null && Object.ToString().Trim().EndsWith("}") && Object.ToString().Trim().StartsWith("{"))
            {
                quot = "";
            }

            return (quot) + (Object == null ? "" : Object.ToString() ) + (quot);
        }

        static bool typeOf(Type t, params Type[] types)
        {
            foreach (Type type in types)
            {
                if (t.Equals(type)) { return true; }
            }
            return false;
        }

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        static bool haSCustomModel(Type type)
        {
            object[] attrs = type.GetCustomAttributes(true);

            foreach (object attr in attrs)
            {
                if (attr.GetType().Equals(typeof(CustomModel)))
                    return true;
            }

            return false;
        }
    }
}