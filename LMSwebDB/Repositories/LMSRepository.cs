using Microsoft.EntityFrameworkCore;

namespace LMSwebDB.Repositories;

public class LMSRepository
{
    private readonly DbContext _context;

    public LMSRepository(DbContext context)
    {
        _context = context;
    }

    public void Create<T>(T entity) where T : class
    {
        _context.Entry(entity).State = EntityState.Added;
    }

    //新增多筆資料
    public void CreateMany<T>(IEnumerable<T> entities) where T : class
    {
        _context.Set<T>().AddRange(entities);
    }

    public void Update<T>(T entity) where T : class
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Entry(entity).State = EntityState.Deleted;
    }

    public void DeleteMany<T>(IEnumerable<T> entities) where T : class
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public IQueryable<T> Query<T>() where T : class
    {
        return _context.Set<T>();
    }
}
