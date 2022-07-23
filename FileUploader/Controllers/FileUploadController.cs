using FileUploader.DAL.Queries;
using FileUploader.Models;
using FileUploader.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private FileValidation _settings;
        private readonly IMediator _mediator;

        public FileUploadController(IOptions<FileValidation> settings, IMediator mediator)
        {
            _settings = settings.Value;
            _mediator = mediator;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var processor = ProcessorFactory.GetProcessorInstance(Path.GetExtension(file.FileName).TrimStart('.'), _mediator);

                    using (Stream stream = file.OpenReadStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string data = await reader.ReadToEndAsync();

                            processor.ProcessFile(data);
                        }
                    }

                    return Ok();
                }
                else
                {

                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error: {ex} ");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var query = new GetTransactionsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
