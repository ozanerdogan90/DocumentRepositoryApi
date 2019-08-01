using DocumentRepositoryApi.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public interface IDocumentRepository
    {
        Task<Guid> Add(Document document);
        Task<Document> Get(Guid id);
        Task<List<Document>> GetAll(string owner = "");
        Task<bool> Update(Document document);
        Task<bool> Delete(Guid id);
    }

    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _context;
        public DocumentRepository(DocumentContext context)
        {
            _context = context;
        }
        public async Task<Guid> Add(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            _context.Documents.Remove(new Document() { Id = id });
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Document> Get(Guid id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<List<Document>> GetAll(string owner = "")
        {
            if (string.IsNullOrEmpty(owner))
                return await _context.Documents.ToListAsync();

            return await _context.Documents.Where(x => x.Owner.Equals(owner, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        }

        public async Task<bool> Update(Document document)
        {
            _context.Update(document);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
