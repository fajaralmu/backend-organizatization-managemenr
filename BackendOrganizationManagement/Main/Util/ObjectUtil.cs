using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BackendOrganizationManagement.Main.Util
{
    public class ObjectUtil
    {

        public static object GetObjectValues(string[] Props, object OriginalObj)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());
            for (int i = 0; i < Props.Length; i++)
            {
                string PropName = Props[i];
                if (HasProperty(PropName, OriginalObj))
                {
                    object val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                    NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);
                }
            }
            return NewObject;
        }

        public static object CopyObjectIgnore(object OriginalObj, params string[] ignore)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());
            PropertyInfo[] Props = NewObject.GetType().GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                string PropName = Props[i].Name;
                bool ignored = false;
                foreach (string ignoredField in ignore)
                {
                    if (PropName.Equals(ignoredField))
                    {
                        ignored = true;
                    }
                }
                if (ignored)
                {
                    continue;
                }

                object val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);

            }
            return NewObject;
        }

        public static bool HasProperty(string PropName, object O)
        {
            foreach (PropertyInfo Prop in O.GetType().GetProperties())
            {
                if (Prop.Name.Equals(PropName))
                {
                    return true;
                }
            }
            return false;
        }

        public static object ConvertList(List<object> value, Type type)
        {
            IList list = (IList)Activator.CreateInstance(type);
            foreach (var item in value)
            {
                list.Add(item);
            }
            return list;
        }
    }
}