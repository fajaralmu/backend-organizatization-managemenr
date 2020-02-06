using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BackendOrganizationManagement.Models;
using BackendOrganizationManagement.Main.Util;
using BackendOrganizationManagement.Main.Dto;

namespace BackendOrganizationManagement.Main.Service
{
    public class PostService
        : BaseService
    {

        public override List<object> ObjectList(int offset, int limit)
        {
            dbEntities = new mpi_dbEntities();
            List<object> ObjList = new List<object>();
            var Sql = (from p in dbEntities.posts orderby p.title select p);
            List<post> List = Sql.Skip(offset * limit).Take(limit).ToList();
            foreach (post c in List)
            {
                ObjList.Add(c);
            }
            count = dbEntities.posts.Count();
            return ObjList;
        }
        public override BaseEntity Update(object Obj)
        {
            dbEntities = new mpi_dbEntities();
            post post = (post)Obj;
            post DBpost = (post)GetById(post.id);
            if (DBpost == null)
            {
                return null;
            }
            dbEntities.Entry(DBpost).CurrentValues.SetValues(post);
            dbEntities.SaveChanges();
            return post;
        }

        public override BaseEntity GetById(object Id)
        {
            dbEntities = new mpi_dbEntities();
            post post = (from c in dbEntities.posts where c.id == (int)Id select c).SingleOrDefault();
            return post;
        }

        public override bool Delete(object Obj)
        {
            try
            {
                post post = (post)Obj;
                dbEntities = new mpi_dbEntities();
                dbEntities.posts.Remove(post);
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
            return count;// dbEntities.posts.Count();
        }

        public override BaseEntity Add(object Obj)
        {
            post post = (post)Obj;

            dbEntities = new mpi_dbEntities();
            post.created_date = DateTime.Now;
            post newpost = dbEntities.posts.Add(post);
            try
            {
                dbEntities.SaveChanges();
                return newpost;
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

        public post findLatestPost()
        {
            dbEntities = new mpi_dbEntities();
            Dictionary<string, object> filter = new Dictionary<string, object>();
            filter.Add("orderby", "post.created_date");
            filter.Add("ordertype", "DESC");
            List<object> posts = SearchAdvanced(filter, 1, 0 ,false);
            if (posts == null || posts.Count == 0)
            {
                return null;
            }
            post Post = (post)posts[0];//.SingleOrDefault();
                                       //    return postObj.SingleOrDefault();
            return Post;
        }

        public override  List<object>  SqlList(string sql, int limit = 0, int offset = 0)
        {
            List<object> categoryList = new List<object>();
            dbEntities = new mpi_dbEntities();
            var posts = dbEntities.posts
                .SqlQuery(sql
                ).
                Select(post => new
                {
                    post
                });
            if (posts != null)
            {
                if (limit > 0)
                {
                    posts = posts.Skip(offset * limit).Take(limit);
                    if(posts == null)
                    {

                        return new List<object>();
                    }
                    posts = posts.ToList();
                }
                else posts = posts.ToList();

                foreach (var u in posts)
                {
                    post post = u.post;
                    categoryList.Add(post);
                }

                return categoryList;
            }
            count = countSQL(sql, dbEntities.posts);
            return new List<object>();

        }
 


        public override int countSQL(string sql, object dbSet)
        {
            return ((DbSet<post>)dbSet)
                .SqlQuery(sql).Count();
        }
         
    }
}