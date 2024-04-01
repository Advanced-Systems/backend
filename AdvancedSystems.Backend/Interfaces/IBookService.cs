using System.Collections.Generic;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Models;

namespace AdvancedSystems.Backend.Interfaces
{
    public interface IBookService
    {
        Task Add(Book book);

        Task<IEnumerable<Book>> GetAllAsync();

        Task<Book?> GetByIdAsync(int id);

        Task Update(int id, Book book);

        Task Delete(int id);
    }
}
