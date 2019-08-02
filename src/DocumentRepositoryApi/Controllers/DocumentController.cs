using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocumentRepositoryApi.Controllers
{
    /// <summary>
    /// Document controller
    /// </summary>
    [Produces("application/json")]
    [Route("documents")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Document model</returns>
        /// <param name="id"></param>
        /// <response code="200">If operation is completed successfuly</response>
        /// <response code="400">If document doesnt exist</response>  
        /// <response code="500">If something goes wrong</response>  
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([NotEmptyGuid]Guid id)
        {
            var doc = await _service.Get(id);
            if (doc == null)
                return NotFound();

            return Ok(doc);
        }

        /// <summary>
        /// Creates a new document
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Document link</returns>
        /// <param name="document"></param>
        /// <response code="201">If operation is completed successfuly</response>
        /// <response code="500">If something goes wrong</response>  
        //// todo hal doc
        [HttpPost]
        public async Task<IActionResult> Post([BindRequired, FromBody] Document document)
        {
            var id = await _service.Add(document);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

        /// <summary>
        /// Modify the existing document
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Document model</returns>
        /// <param name="id"></param>
        /// <param name="document"></param>
        /// <response code="200">If operation is completed successfuly</response>
        /// <response code="400">If document doesnt exist</response>  
        /// <response code="500">If something goes wrong</response>  
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([NotEmptyGuid]Guid id, [BindRequired, FromBody] Document document)
        {
            var doc = await _service.Get(id);
            if (doc == null)
                return NotFound();

            var updated = await _service.Update(id, document);
            if (!updated)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok(document);
        }

        /// <summary>
        /// Delete existing document
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id"></param>
        /// <response code="200">If operation is completed successfuly</response>
        /// <response code="500">If something goes wrong</response>  
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([NotEmptyGuid]Guid id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
