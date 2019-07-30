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
    }

    public class DocumentService
    {
    }
}
