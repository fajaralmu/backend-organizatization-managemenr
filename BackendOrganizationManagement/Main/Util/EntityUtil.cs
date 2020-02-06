using BackendOrganizationManagement.Main.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BackendOrganizationManagement.Main.Util
{
    public class EntityUtil
    {


        internal static List<PropertyInfo> getDeclaredField(Type clazz, string orderBy = "")
        {
            PropertyInfo[] baseField = clazz.GetProperties();

            List<PropertyInfo> fieldList = new List<PropertyInfo>();
            foreach (PropertyInfo field in baseField)
            {
                fieldList.Add(field);
            }
            if (clazz.BaseType != null)
            {
                PropertyInfo[] parentFields = clazz.BaseType.GetProperties();
                foreach (PropertyInfo field in parentFields)
                {
                    fieldList.Add(field);
                }

            }
            return fieldList;
        }

        public static PropertyInfo getIdField(Type clazz)
        {
            if (clazz.GetCustomAttribute(typeof(CustomModel)) == null)
            {
                return null;
            }
            List<PropertyInfo> fields = getDeclaredField(clazz);
            foreach (PropertyInfo field in fields)
            {
                if (field.GetCustomAttribute(typeof(Id)) != null)
                {
                    return field;
                }
            }

            return null;
        }

        public static PropertyInfo getSingleDeclaredField(Type clazz, String fieldName)
        {

            PropertyInfo field = clazz.GetProperty(fieldName);
            if (field != null)
            {
                return field;
            }
            if (clazz.BaseType != null)
            {
                return clazz.BaseType.GetProperty(fieldName);
            }

            return null;
        }
    }
}