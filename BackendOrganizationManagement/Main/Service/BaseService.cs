using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Handler;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Service
{
    public abstract class BaseService
    {
        public int count = 0;
        protected static mpi_dbEntities dbEntities;
        protected SessionService sessionService;

        public BaseService()
        {
            sessionService = new SessionService();
        }

        public abstract List<object> ObjectList(int offset, int limit);

        public abstract BaseEntity Update(object obj);

        public abstract BaseEntity GetById(object Id);

        public abstract bool Delete(object obj);

        public abstract int ObjectCount();

        public abstract BaseEntity Add(object Obj)
        ;

        public virtual List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        { return null; }

        public abstract int countSQL(string sql, object dbSet)
        ;

        public virtual int getCountSearch()
        {
            return 0;
        }

        public static List<object> GetObjectList(BaseService Service, HttpRequestBase Req, user LoggedUser = null)
        {
            //Service.Refresh();
            int Offset = 0;
            int Limit = 0;
            Dictionary<string, object> Params = new Dictionary<string, object>();
            if (StringUtil.NotNullAndNotBlank(Req.Form["limit"]) && StringUtil.NotNullAndNotBlank(Req.Form["offset"]))
            {
                try
                {
                    Offset = int.Parse(Req.Form["offset"].ToString());
                    Limit = int.Parse(Req.Form["limit"].ToString());

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            if (StringUtil.NotNullAndNotBlank(Req.Form["search_param"]))
            {
                string Param = Req.Form["search_param"].ToString();
                Param = Param.Replace("${", "");
                Param = Param.Replace("}$", "");
                Param = Param.Replace(";", "&");


                Params = StringUtil.QUeryStringToDict(Param);
            }
            if (LoggedUser != null)
            {
                Params.Add("institution_id", LoggedUser.institution_id);
            }
            return Service.SearchAdvanced(Params, Limit, Offset);

        }

        public static Dictionary<string, object> ReqToDict(HttpRequestBase Req)
        {
            Dictionary<string, object> Map = new Dictionary<string, object>();
            foreach (string Key in Req.Form.Keys)
            {
                if (Key.StartsWith("field-param-"))
                {
                    string DictKey = Key.Replace("field-param-", "");
                    Map.Add(DictKey, Req.Form[Key]);
                }
            }


            return Map;
        }


        public abstract List<object> SqlList(string sql, int limit = 0, int offset = 0);

        //protected void Refresh()
        //{
        //    if (dbEntities != null)
        //        dbEntities.Dispose();
        //    dbEntities = mpi_dbEntities.Instance();

        //}

    }
}