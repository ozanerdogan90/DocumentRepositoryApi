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
        private readonly IMapper _mapper;
        public DocumentService(IDocumentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Guid> Add(Document document)
        {
            var entity = _mapper.Map<DocumentEntity>(document);
            return await _repo.Add(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repo.Delete(id);
        }

        public async Task<Document> Get(Guid id)
        {
            var entity = await _repo.Get(id);
            return _mapper.Map<Document>(entity);
        }

        public async Task<List<Document>> GetAll(string owner = "")
        {
            var entities = await _repo.GetAll(owner);
            return _mapper.Map<List<Document>>(entities);
        }

        public async Task<bool> Update(Guid id, Document document)
        {
            var entity = _mapper.Map<DocumentEntity>(document);
            entity.Id = id;
            return await _repo.Update(entity);
        }
    }
}
