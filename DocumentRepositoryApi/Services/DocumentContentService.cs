using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public interface IDocumentContentService
    {
        Task<List<Guid>> GetLinks(Guid documentId);
        Task<object> Get(Guid documentId);
    }

    public class DocumentContentService : IDocumentContentService
    {
        public Task<object> Get(Guid documentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> GetLinks(Guid documentId)
        {
            throw new NotImplementedException();
        }
    }
}
