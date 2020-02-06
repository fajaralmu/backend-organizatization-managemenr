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
    public class ProgramService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var Sql = (from p in dbEntities.programs orderby p.name select p);
            List<program> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (program c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.programs.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            program program = (program)Obj;
            program DBProgram = (program)GetById(program.id);
            if (DBProgram == null)
            {
                return null;
            }
            dbEntities.Entry(DBProgram).CurrentValues.SetValues(program);
            dbEntities.SaveChanges();
            return program;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            program program = (from c in dbEntities.programs where c.id == (int)Id select c).SingleOrDefault();
            return program;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                program program = (program)Obj;
                dbEntities.programs.Remove(program);
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
            return count;// dbEntities.programs.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            program program = (program)Obj;
            dbEntities = new mpi_dbEntities();
            program newProgram = dbEntities.programs.Add(program);
            try
            {
                dbEntities.SaveChanges();
                return newProgram;
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
            var programs = dbEntities.programs
                .SqlQuery(sql
                ).
                Select(program => new
                {
                    program
                });
            if (limit > 0)
            {
                programs = programs.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                programs = programs.ToList();
            }
            foreach (var u in programs)
            {
                program program = u.program;
                categoryList.Add(program);
            }
            count = countSQL(sql, dbEntities.programs);
            return categoryList;
        }
 
        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<program>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}