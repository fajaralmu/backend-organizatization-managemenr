using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Models;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Main.Dto;

namespace BackendOrganizationManagement.Main.Service
{
    public class InstitutionService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var Sql = (from p in dbEntities.institutions orderby p.name select p);
            List<institution> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (institution c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.institutions.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            institution institution = (institution)Obj;
            institution DBinstitution = (institution)GetById(institution.id);
            if (DBinstitution == null)
            {
                return null;
            }
            dbEntities.Entry(DBinstitution).CurrentValues.SetValues(institution);
            dbEntities.SaveChanges();
            return institution;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            institution institution = (from c in dbEntities.institutions where c.id == (int)Id select c).SingleOrDefault();
            return institution;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                institution institution = (institution)Obj;
                dbEntities.institutions.Remove(institution);
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
            return count;// dbEntities.institutions.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            institution institution = (institution)Obj;
            dbEntities = new mpi_dbEntities();
            institution newinstitution = dbEntities.institutions.Add(institution);
            try
            {
                dbEntities.SaveChanges();
                return newinstitution;
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
            var institutions = dbEntities.institutions
                .SqlQuery(sql
                ).
                Select(institution => new
                {
                    institution
                });
            if (limit > 0)
            {
                institutions = institutions.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                institutions = institutions.ToList();
            }
            foreach (var u in institutions)
            {
                institution institution = u.institution;
                categoryList.Add(institution);
            }
            count = countSQL(sql, dbEntities.institutions);
            return categoryList;
        } 

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<institution>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}