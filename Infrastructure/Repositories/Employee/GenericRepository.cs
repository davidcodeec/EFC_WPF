using Infrastructure.Contexts;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Employee;

public abstract class GenericRepository<TEntity> where TEntity : class
{
    protected readonly EmployeeDbContext _employeeDbContext;
    protected readonly ILogs _logs;

    protected GenericRepository(EmployeeDbContext employeeDbContext, ILogs logs)
    {
        _employeeDbContext = employeeDbContext;
        _logs = logs;
    }


    // IF EXISTS (SELECT 1 FROM Table WHERE Id = 1)
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var found = await _employeeDbContext.Set<TEntity>().AnyAsync(predicate);
            return found;
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - ExistsAsync"); }
        return false;
    }

    public virtual async Task<TEntity?> CreateAsync(TEntity entity)
    {
        try
        {
           
            var entityState = _employeeDbContext.Entry(entity).State;
            

            _employeeDbContext.Set<TEntity>().Add(entity);
            

            await _employeeDbContext.SaveChangesAsync();
           

            return entity;
        }
        catch (Exception ex)
        {
            
            Debug.WriteLine(ex.ToString());

            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - CreateAsync");
        }

        
        return null!;
    }




    // SELECT * FROM Table
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _employeeDbContext.Set<TEntity>().ToListAsync();
            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - GetAllAsync"); }
        return null!;
    }


    // SELECT TOP(5) * FROM Table
    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, int take)
    {
        try
        {
            var entities = await _employeeDbContext.Set<TEntity>().Where(predicate).Take(take).ToListAsync();
            if (entities.Count != 0)
            {
                return entities;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - GetAsync"); }
        return null!;
    }


    // SELECT * FROM Table WHERE Id = 1
    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - GetOneAsync"); }
        return null!;
    }


    // UPDATE Table SET ColumnName_1 = Value_1, ColumnName_2 = Value_2 WHERE Id = 1
    public virtual async Task<TEntity?> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        try
        {
            

            var entity = await _employeeDbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                

                var entityType = typeof(TEntity);
                var keyProperties = _employeeDbContext.Model.FindEntityType(entityType).FindPrimaryKey().Properties;

                foreach (var property in keyProperties)
                {
                   
                    _employeeDbContext.Entry(entity).Property(property.Name).IsModified = false;
                }

                
                _employeeDbContext.Entry(entity).CurrentValues.SetValues(updatedEntity);

               
                await _employeeDbContext.SaveChangesAsync();

                
                return entity;
            }
            else
            {
                
                return null;
            }
        }
        catch (Exception ex)
        {
            
            Debug.WriteLine(ex.ToString());

            await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - UpdateAsync");
            throw;
        }
    }




    // DELETE FROM Table WHERE Id = 1
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _employeeDbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _employeeDbContext.Set<TEntity>().Remove(entity);
                await _employeeDbContext.SaveChangesAsync();

                return true;
            }
        }
        catch (Exception ex) { await _logs.LogToFileAsync(ex.ToString(), $"{nameof(GenericRepository<TEntity>)} - UpdateAsync"); }
        return false;
    }

}
