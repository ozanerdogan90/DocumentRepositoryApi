using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{
    [Route("documents/{documentId}/contents")]
    [ApiController]
    public class DocumentContentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentContentService _contentService;
        public DocumentContentController(IDocumentService documentService, IDocumentContentService contentService)
        {
            _documentService = documentService;
            _contentService = contentService;
        }

        //// TODO: not guid id 
        [HttpGet("links")]
        public async Task<IActionResult> Get([BindRequired]Guid documentId)
        {
            var doc = await _documentService.Get(documentId);
            if (doc == null)
                return NotFound();

            var links = await _contentService.GetLinks(documentId);

            return Ok(links);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([BindRequired]Guid documentId, [BindRequired]Guid id)
        {
            var doc = await _documentService.Get(documentId);
            if (doc == null)
                return NotFound();

            var file = await _contentService.Get(id);

            return Ok(file);
        }
    }
}
