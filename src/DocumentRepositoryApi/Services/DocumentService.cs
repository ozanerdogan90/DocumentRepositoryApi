using AutoMapper;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentEntity = DocumentRepositoryApi.DataAccess.Entities.Document;

namespace DocumentRepositoryApi.Services
{
    public interface IDocumentService
    {
        Task<Guid> Add(Document document);
        Task<Document> Get(Guid id);
        Task<List<Document>> GetAll(string owner = "");
        Task<bool> Update(Guid id, Document document);
        Task<bool> Delete(Guid id);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repo;
        public DocumentService(IDocumentRepository repo)
        {
            _repo = repo;
        }


        public async Task<Guid> Add(Document document)
        {
            var entity = Mapper.Map<DocumentEntity>(document);
            return await _repo.Add(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repo.Delete(id);
        }

        public async Task<Document> Get(Guid id)
        {
            var entity = await _repo.Get(id);
            return Mapper.Map<Document>(entity);
        }

        public async Task<List<Document>> GetAll(string owner = "")
        {
            var entities = await _repo.GetAll(owner);
            return Mapper.Map<List<Document>>(entities);
        }

        public async Task<bool> Update(Guid id, Document document)
        {
            var entity = Mapper.Map<DocumentEntity>(document);
            entity.Id = id;
            return await _repo.Update(entity);
        }
    }
}
