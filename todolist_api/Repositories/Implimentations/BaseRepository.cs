using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Models;
using Task = System.Threading.Tasks.Task;

namespace todolist_api.Repositories
{
    public class BaseRepository<TDbModel> : IBaseRepository<TDbModel> where TDbModel : BaseModel
    {
        private readonly ApplicationContext _context;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TDbModel> Create(TDbModel model)
        {
            await _context.Set<TDbModel>().AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task Delete(int id)
        {
            var toDelete = await _context.Set<TDbModel>().FirstOrDefaultAsync(m => m.Id == id) ?? throw new ArgumentException("Record not found");

            _context.Set<TDbModel>().Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<TDbModel> Get(int id)
        {
            return await _context.Set<TDbModel>().FirstOrDefaultAsync(m => m.Id == id) ?? throw new ArgumentException("Record not found");
        }

        public async Task<List<TDbModel>> GetAll()
        {
            return await _context.Set<TDbModel>().ToListAsync();
        }

        public async Task<TDbModel> Update(TDbModel model)
        {
            var toUpdate = await _context.Set<TDbModel>().FirstOrDefaultAsync(m => m.Id == model.Id) ?? throw new ArgumentException("Record not found");
            
            _context.Entry(toUpdate).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();

            return toUpdate;
        }
    }
}