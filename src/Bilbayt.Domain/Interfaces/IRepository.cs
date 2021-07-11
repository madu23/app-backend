using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bilbayt.Domain.Interfaces
{
    public interface IDataContext
    {
        IQueryable<T> All<T>() where T : class, new();
        Task<T> GetDocument<T>(FilterDefinition<T> filter);
        IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task DeleteRecord<T>(FilterDefinition<T> filter);
        void Add<T>(T item) where T : class, new();
        Task Update<T>(FilterDefinition<T> filter, KeyValuePair<string, object> property);
        Task UpdateRecord<T>(string id, T record);
        Task<List<T>> GetDocuments<T>(FilterDefinition<T> filter);
    }
}
