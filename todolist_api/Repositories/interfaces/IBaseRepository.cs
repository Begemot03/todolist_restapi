using todolist_api.Models;
using Task = System.Threading.Tasks.Task;

namespace todolist_api.Repositories
{
    public interface IBaseRepository<TDbModel> where TDbModel : BaseModel
    {
        public Task<List<TDbModel>> GetAll();
        public Task<TDbModel> Get(int id);
        public Task<TDbModel> Create(TDbModel model);
        public Task<TDbModel> Update(TDbModel model);
        public Task Delete(int id);
    }
}