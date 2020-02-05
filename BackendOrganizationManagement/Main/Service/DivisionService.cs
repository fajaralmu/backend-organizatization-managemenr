using BackendOrganizationManagement.Main.Dto;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Service
{
    public class DivisionService : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var Sql = (from p in dbEntities.divisions orderby p.name select p);
            List<division> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (division c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.divisions.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            division division = (division)Obj;
            division DBDivision = (division)GetById(division.id);
            if (DBDivision == null)
            {
                return null;
            }
            dbEntities.Entry(DBDivision).CurrentValues.SetValues(division);
            dbEntities.SaveChanges();
            return division;
        }

        public override BaseEntity GetById(object Id)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                division division = (from c in dbEntities.divisions where c.id == (int)Id select c).SingleOrDefault();
                return division;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                division division = (division)Obj;
                dbEntities.divisions.Remove(division);
                dbEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }




        public override int ObjectCount()
        {
            return count;// dbEntities.divisions.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            division division = (division)Obj;
            dbEntities = new mpi_dbEntities();
            division newDivision = dbEntities.divisions.Add(division);
            try
            {
                dbEntities.SaveChanges();
                return newDivision;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //  return null;
            }

        }

        public override List<object> SqlList(string sql, int limit = 0, int offset = 0)
        {
            List<object> categoryList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var divisions = dbEntities.divisions
                .SqlQuery(sql
                ).
                Select(division => new
                {
                    division
                });
            if (limit > 0)
            {
                divisions = divisions.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                divisions = divisions.ToList();
            }
            foreach (var u in divisions)
            {
                division division = u.division;
                categoryList.Add(division);
            }

            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        {

            string id = Params.ContainsKey("id") ? Params["id"].ToString() : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string desc = Params.ContainsKey("description") ? (string)Params["description"] : "";
            string institution_id = Params.ContainsKey("institution_id") ? Params["institution_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from [division] where [id] like '%" + id + "%'" +
                " and [name] like '%" + name + "%' and [description] like '%" + desc + "%'" +
                (StringUtil.NotNullAndNotBlank(institution_id) ? " and [institution_id] =" + institution_id + " " : "");
            sql += StringUtil.AddSortQuery(orderby, ordertype);
            dbEntities = new mpi_dbEntities();
            count = countSQL(sql, dbEntities.divisions);
            return SqlList(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<division>)dbSet)
                .SqlQuery(sql).Count();
        }

        public override int getCountSearch()
        {
            throw new NotImplementedException();
        }

         
    }
}