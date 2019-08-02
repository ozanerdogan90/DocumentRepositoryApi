using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentRepositoryApi.DataAccess.Entities;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public class InMemoryStorageRepository : IDocumentContentRepository
    {
        private readonly Dictionary<Guid, DocumentContent> fileDictionary;
        public InMemoryStorageRepository()
        {
            fileDictionary = new Dictionary<Guid, DocumentContent>();
        }
        public async Task<DocumentContent> Fetch(Guid id)
        {
            if (fileDictionary.TryGetValue(id, out DocumentContent doc))
            {
                return doc;
            }

            return null;
        }

        public async Task<bool> Store(DocumentContent content)
        {
            return fileDictionary.TryAdd(content.DocumentId, content);
        }
    }
}
