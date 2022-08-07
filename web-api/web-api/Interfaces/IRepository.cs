using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api.Models;

namespace web_api.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int objId);
        Task<IEnumerable<T>> Get();
        Task<bool> Create(T obj);
        Task<bool> Update(T obj);
        Task<bool> Delete(int objId);
    }
}
