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
    public class EventService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new mpi_dbEntities();
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.events orderby p.name select p);
            List<@event> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (@event c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.events.Count();
            return ObjList;
        }

        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            @event @event = (@event)Obj;
            @event.created_date = DateTime.Now;
            @event DBEvent = (@event)GetById(@event.id);
            if (DBEvent == null)
            {
                return null;
            }
            dbEntities.Entry(DBEvent).CurrentValues.SetValues(@event);
            dbEntities.SaveChanges();
            return @event;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            @event @event = (from o in dbEntities.events where o.id == (int)Id select o).SingleOrDefault();
            return @event;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                @event @event = (@event)Obj;
                dbEntities.events.Remove(@event);
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
            return count;// dbEntities.events.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            @event @event = (@event)Obj;
            dbEntities = new mpi_dbEntities();
            @event.created_date = DateTime
                .Now;
            @event newEvent = dbEntities.events.Add(@event);
            try
            {
                dbEntities.SaveChanges();
                return newEvent;
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
            var Events = dbEntities.events
                .SqlQuery(sql
                ).
                Select(@event => new
                {
                    @event
                });
            if (limit > 0)
            {
                Events = Events.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                Events = Events.ToList();
            }
            foreach (var u in Events)
            {
                @event @event = u.@event;
                categoryList.Add(@event);
            }
            count = countSQL(sql, dbEntities.events);
            return categoryList;
        } 
        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<@event>)dbSet)
                .SqlQuery(sql).Count();
        }

        public override int getCountSearch()
        {
            throw new NotImplementedException();
        }
    }
}