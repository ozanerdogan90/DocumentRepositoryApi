using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentContentEntity = DocumentRepositoryApi.DataAccess.Entities.DocumentContent;

namespace DocumentRepositoryApi.Services
{
    public interface IDocumentContentService
    {
        Task<DocumentContent> Get(Guid documentId);
        Task<bool> Store(Guid documentId, IFormFile file);
    }

    public class DocumentContentService : IDocumentContentService
    {
        private readonly ICompressionService _compressionService;
        private readonly IEncryptionService _encryptionService;
        private readonly IDocumentContentRepository _repo;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;
        public DocumentContentService(ICompressionService compressionService, IEncryptionService encryptionService, IDocumentContentRepository repo)
        {
            _compressionService = compressionService;
            _encryptionService = encryptionService;
            _repo = repo;
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        public async Task<DocumentContent> Get(Guid documentId)
        {
            var result = await _repo.Fetch(documentId);
            var decryptedContent = _encryptionService.Decrypt(result.Content);
            return new DocumentContent()
            {
                Content = _compressionService.Decompress(decryptedContent),
                ContentType = result.Type,
                Name = result.Name
            };
        }


        public async Task<bool> Store(Guid documentId, IFormFile file)
        {
            var compressedFile = _compressionService.Compress(file);
            var encryptedFile = _encryptionService.Encrypt(compressedFile);
            _contentTypeProvider.TryGetContentType(file.FileName, out string contentType);
            var documentContent = new DocumentContentEntity()
            {
                DocumentId = documentId,
                Content = encryptedFile,
                Length = encryptedFile.Length,
                Name = file.FileName,
                Type = contentType
            };

            return await _repo.Store(documentContent);

        }
    }
}
