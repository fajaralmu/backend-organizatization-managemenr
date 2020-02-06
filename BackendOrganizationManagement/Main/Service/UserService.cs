using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BackendOrganizationManagement.Models;
using OrgWebMvc.Main.Util;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Main.Dto;

namespace BackendOrganizationManagement.Main.Service
{
    public class UserService : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            List<object> ObjList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var Sql = (from p in dbEntities.users orderby p.name select p);
            List<user> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (user c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.users.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            user user = (user)Obj;
            user DBUser = (user)GetById(user.id);
            if (DBUser == null)
            {
                return null;
            }
            dbEntities.Entry(DBUser).CurrentValues.SetValues(user);
            dbEntities.SaveChanges();
            return user;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            user user = (from c in dbEntities.users where c.id == (int)Id select c).SingleOrDefault();
            return user;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                user user = (user)Obj;
                dbEntities = new mpi_dbEntities();
                dbEntities.users.Remove(user);
                dbEntities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public user GetUserByUsernameAndPassword(string Username, string Password)
        {
            DebugConsole.Debug(this,"DB ENTITIES IS NULL: "+ (dbEntities==null));
            dbEntities = new mpi_dbEntities();
            if (dbEntities == null || Username == null || Password == null)
            {
                return null;
            }
          
            DebugConsole.Debug(this, dbEntities.users.ToString());
            user User = (from u in dbEntities.users
                         where u.username.Equals(Username) && u.password.Equals(Password)
                         select u).SingleOrDefault();
            if (User != null)
            {
                return User;
            }
            return null;
        }

        public override int ObjectCount()
        {
            return count;// dbEntities.users.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            user user = (user)Obj;
            if (user.admin == null)
            {
                user.admin = 1;
            }

            dbEntities = new mpi_dbEntities();
            user newUser = dbEntities.users.Add(user);
            try
            {
                dbEntities.SaveChanges();
                return newUser;
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
            var users = dbEntities.users
                .SqlQuery(sql
                ).
                Select(user => new
                {
                    user
                });
            if (limit > 0)
            {
                users = users.Skip(offset * limit).Take(limit).ToList();
            }
            else
            {
                users = users.ToList();
            }
            foreach (var u in users)
            {
                user User = u.user;
                categoryList.Add(User);
            }
            count = countSQL(sql, dbEntities.users);
            return categoryList;
        }

        public override List<object> SearchAdvanced(Dictionary<string, object> Params, int limit = 0, int offset = 0, bool updateCount = true)
        {

            string id = Params.ContainsKey("id") ? (string)Params["id"] : "";
            string name = Params.ContainsKey("name") ? (string)Params["name"] : "";
            string institution_id = Params.ContainsKey("institution_id") ? Params["institution_id"].ToString() : "";
            string orderby = Params.ContainsKey("orderby") ? (string)Params["orderby"] : "";
            string ordertype = Params.ContainsKey("ordertype") ? (string)Params["ordertype"] : "";

            string sql = "select * from [user] where [id] like '%" + id + "%'" +
                " and [name] like '%" + name + "%' " + (StringUtil.NotNullAndNotBlank(institution_id) ? " and [institution_id]=" + institution_id + " " : "");
            ;
            sql += StringUtil.AddSortQuery(orderby, ordertype);
            dbEntities = new mpi_dbEntities();
            count = countSQL(sql, dbEntities.users);
            return SqlList(sql, limit, offset);
        }


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<user>)dbSet)
                .SqlQuery(sql).Count();
        }

    }
}