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

    public class DocumentContentService
    {
    }
}
