﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSD.Util.Repository
{
    public class CrudRepository<Entity, ID, Context> : ICrudRepository<Entity, ID>
        where Entity : class, IEntity<ID>
        where Context : DbContext
    {
        private readonly DbSet<Entity> m_entities;       

        public Context Ctx { get; }

        public CrudRepository(Context context)
        {
            Ctx = context;
            m_entities = Ctx.Set<Entity>();
        }

        #region Sync methods

        public IEnumerable<Entity> All => m_entities.ToList();        

        public long Count() => m_entities.LongCount();        

        public void Delete(Entity entity) => DeleteById(entity.Id);

        public void DeleteById(ID id)
        {          
            var e = FindById(id);

            if (e == null)
                return;

            m_entities.Remove(e);
            Ctx.SaveChanges();
        }

        public bool ExistsById(ID id) => FindById(id) != null;

        public IEnumerable<Entity> FindBy(Expression<Func<Entity, bool>> predicate) => m_entities.Where(predicate).ToList();        

        public Entity FindById(ID id) => m_entities.FirstOrDefault(e => e.Id.Equals(id));        

        public IEnumerable<Entity> FindByIds(IEnumerable<ID> ids)
        {
            throw new NotImplementedException();
        }

        public Entity Save(Entity entity)
        {
            if (ExistsById(entity.Id))
                m_entities.Update(entity);
            else
                m_entities.Add(entity);

            Ctx.SaveChanges();

            return entity;
        }

        public IEnumerable<Entity> Save(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Async methods

        public Task<IEnumerable<Entity>> FindAllAsync()
        {
            var task = new Task<IEnumerable<Entity>>(() => m_entities.ToList());

            task.Start();

            return task;
        }

        public Task<long> CountAsync() => m_entities.LongCountAsync();

        public Task DeleteAysnc(Entity entity) => DeleteByIdAsync(entity.Id);        

        public Task DeleteByIdAsync(ID id)
        {
            var task = new Task(() => DeleteById(id));

            task.Start();

            return task;
        }

        public Task<bool> ExistsByIdAsync(ID id)
        {
            var task = new Task<bool>(() => FindByIdAsync(id).Result != null);

            task.Start();

            return task;
        }
        public Task<IEnumerable<Entity>> FindByAsync(Expression<Func<Entity, bool>> predicate)
        {
            var task = new Task<IEnumerable<Entity>>(() => m_entities.Where(predicate).ToList());

            task.Start();

            return task;
        }

        public Task<Entity> FindByIdAsync(ID id) => m_entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        
        public Task<IEnumerable<Entity>> FindByIdsAsync(IEnumerable<ID> ids)
        {
            throw new NotImplementedException();
        }

        public Task<Entity> SaveAsync(Entity entity)
        {
            if (ExistsById(entity.Id))
                m_entities.Update(entity);
            else
                m_entities.Add(entity);

            var task = new Task<Entity>(() => { Ctx.SaveChanges(); return entity; });

            task.Start();

            return task;
        }

        /*
        public async Task<Entity> SaveAsync(Entity entity)
        {
            if (await ExistsByIdAsync(entity.Id))
                m_entities.Update(entity);
            else
                m_entities.Add(entity);

            await Ctx.SaveChangesAsync();

            return entity;
        }
        */
        

        public Task<IEnumerable<Entity>> SaveAsync(IEnumerable<Entity> entities)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
