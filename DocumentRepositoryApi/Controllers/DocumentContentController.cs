using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{
    [Route("documents/{documentId}/contents")]
    [ApiController]
    [Authorize]
    public class DocumentContentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentContentService _contentService;
        public DocumentContentController(IDocumentService documentService, IDocumentContentService contentService)
        {
            _documentService = documentService;
            _contentService = contentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([NotEmptyGuid]Guid documentId)
        {
            var doc = await _documentService.Get(documentId);
            if (doc == null)
                return NotFound();

            var file = await _contentService.Get(documentId);

            return File(file.Content, file.ContentType, file.Name);
        }

        public async Task<IActionResult> Post([NotEmptyGuid]Guid documentId, IFormFile file)
        {
            var doc = await _documentService.Get(documentId);
            if (doc == null)
                return BadRequest($"Document with id:{documentId} doesnt exist");

            var result = await _contentService.Store(documentId, file);
            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }
    }
}
