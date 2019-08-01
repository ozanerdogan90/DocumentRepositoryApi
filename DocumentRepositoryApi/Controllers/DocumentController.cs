using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocumentRepositoryApi.Controllers
{
    [Route("documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        //// TODO: not guid id 
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([BindRequired]Guid id)
        {
            var doc = await _service.Get(id);
            if (doc == null)
                return NotFound();

            return Ok(doc);
        }

        [HttpPost]
        public async Task<IActionResult> Post([BindRequired, FromBody] Document document)
        {
            var id = await _service.Add(document);
            return Created("", id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([BindRequired]Guid id, [BindRequired, FromBody] Document document)
        {
            var doc = await _service.Get(id);
            if (doc == null)
                return NotFound();

            await _service.Update(id, document);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([BindRequired]Guid id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
