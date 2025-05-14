
using LibraryCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCore.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected LibraryDBContext context;
        protected  DbSet<T> dbSet; 
        public GenericRepository(LibraryDBContext context = null)
        {
            this.context = context ?? new LibraryDBContext();
            dbSet = context.Set<T>();
            
            
        }
        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll()
        {
            var query = dbSet.AsQueryable();
            var navigations = context.Model.FindEntityType(typeof(T))
                .GetDerivedTypesInclusive()
                .SelectMany(type => type.GetNavigations())
                .Distinct();
            foreach (var property in navigations)
            {
                query = query.Include(property.Name);
            }
            return query.ToList();
        }

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Add new Entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
            
        }

        /// <summary>
        /// Update an Entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete an Entity, Using entity's id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            T entity = dbSet.Find(id);
            dbSet.Remove(entity);
            
        }
     
    }
}
