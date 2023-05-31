using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedSystems.Backend.Models.Interfaces;

public interface IBookService
{
    Task Add(Book book);

    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetById(int id);

    Task Update(int id, Book book);

    Task Delete(int id);
}
