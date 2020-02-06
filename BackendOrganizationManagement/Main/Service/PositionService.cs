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
    public class PositionService

        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var Sql = (from p in dbEntities.positions orderby p.name select p);
            List<position> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (position c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.positions.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            position position = (position)Obj;
            position DBposition = (position)GetById(position.id);
            if (DBposition == null)
            {
                return null;
            }
            dbEntities.Entry(DBposition).CurrentValues.SetValues(position);
            dbEntities.SaveChanges();
            return position;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            position position = (from c in dbEntities.positions where c.id == (int)Id select c).SingleOrDefault();
            return position;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                position position = (position)Obj;
                dbEntities.positions.Remove(position);
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
            return count;// dbEntities.positions.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            position position = (position)Obj;
            position.created_date = DateTime.Now;
            dbEntities = new mpi_dbEntities();
            position newposition = dbEntities.positions.Add(position);
            try
            {
                dbEntities.SaveChanges();
                return newposition;
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
            var positions = dbEntities.positions
                .SqlQuery(sql
                ).
                Select(position => new
                {
                    position
                });
            if (limit > 0)
            {
                positions = positions.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                positions = positions.ToList();
            }
            foreach (var u in positions)
            {
                position position = u.position;
                categoryList.Add(position);
            }
            count = countSQL(sql, dbEntities.positions);
            return categoryList;
        }

       
        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<position>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}