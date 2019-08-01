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

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([NotEmptyGuid]Guid id)
        {
            var doc = await _service.Get(id);
            if (doc == null)
                return NotFound();

            return Ok(doc);
        }

        //// todo hal doc
        [HttpPost]
        public async Task<IActionResult> Post([BindRequired, FromBody] Document document)
        {
            var id = await _service.Add(document);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([NotEmptyGuid]Guid id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
