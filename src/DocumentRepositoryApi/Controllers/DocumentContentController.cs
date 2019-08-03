using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{
    /// <summary>
    /// Document Content Controller to add new files
    /// </summary>
    [Produces("application/json")]
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

        /// <summary>
        /// Gets file by document id
        /// </summary>
        /// <remarks></remarks>
        /// <param name="documentId"></param>
        /// <returns>Document content</returns>
        /// <response code="200">Returns document content as file</response>
        /// <response code="404">If document doesnt exist</response>  
        /// <response code="500">If something goes wrong</response>  
        [HttpGet]
        public async Task<IActionResult> Get([NotEmptyGuid]Guid documentId)
        {
            var doc = await _documentService.Get(documentId);
            if (doc == null)
                return NotFound();

            var file = await _contentService.Get(documentId);

            return File(file.Content, file.ContentType, file.Name);
        }

        /// <summary>
        /// Creates a new content
        /// </summary>
        /// <remarks></remarks>
        /// <param name="documentId"></param>
        /// <param name="file"></param>
        /// <response code="200">If operation is completed successfuly</response>
        /// <response code="400">If document doesnt exist</response>  
        /// <response code="500">If something goes wrong</response>  
        [HttpPost]
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
