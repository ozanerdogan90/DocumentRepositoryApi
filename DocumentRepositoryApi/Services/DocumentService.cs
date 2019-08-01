using DocumentRepositoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public interface IDocumentService
    {
        Task<Guid> Add(Document document);
        Task<Document> Get(Guid id);
        Task<List<Document>> GetAll();
        Task<bool> Update(Guid id, Document document);
        Task<bool> Delete(Guid id);
    }

    public class DocumentService : IDocumentService
    {
        public Task<Guid> Add(Document document)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Document> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Document>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Guid id, Document document)
        {
            throw new NotImplementedException();
        }
    }
}
