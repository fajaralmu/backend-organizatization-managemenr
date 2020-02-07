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
    public class MemberService

        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new mpi_dbEntities();
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.members orderby p.name select p);
            List<member> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (member c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.members.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            member Member = (member)Obj;
            member DBmember = (member)GetById(Member.id);
            if (DBmember == null)
            {
                return null;
            }
            dbEntities.Entry(DBmember).CurrentValues.SetValues(Member);
            dbEntities.SaveChanges();
            return Member;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            member member = (from c in dbEntities.members where c.id == (int)Id select c).SingleOrDefault();
            return member;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                dbEntities = new mpi_dbEntities();
                member member = (member)Obj;
                dbEntities.members.Remove(member);
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
            return count;// dbEntities.members.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            member member = (member)Obj;
            try
            {
                dbEntities = new mpi_dbEntities();
                member newmember = dbEntities.members.Add(member);

                dbEntities.SaveChanges();
                return newmember;
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

            var members = dbEntities.members
                .SqlQuery(sql
                ).
                Select(member => new
                {
                    member
                });
            if (limit > 0)
            {
                members = members.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                members = members.ToList();
            }
            foreach (var u in members)
            {
                member member = u.member;
                categoryList.Add(member);
            }
            count = countSQL(sql, dbEntities.members);
            return categoryList;
        }
         

        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<member>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}