using DocumentRepositoryApi.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public interface IDocumentContentRepository
    {
        Task<bool> Store(DocumentContent content);
        Task<DocumentContent> Fetch(Guid id);
        Task RemoveAsync(Guid id);
    }
}
