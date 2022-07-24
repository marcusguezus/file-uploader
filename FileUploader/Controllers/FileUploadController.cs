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
                var fileExtension = Path.GetExtension(file.FileName).TrimStart('.');

                if (fileExtension == "csv" || fileExtension == "xml")
                {
                    if (ConvertBytesToMegabytes(file.Length) < 1f)
                    {
                        var processor = ProcessorFactory.GetProcessorInstance(fileExtension, _mediator);

                        using (Stream stream = file.OpenReadStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string data = await reader.ReadToEndAsync();

                                processor.ProcessFile(data);
                            }
                        }

                        return Ok("Upload success");
                    }
                    else
                    {
                        return BadRequest("File must not be more than 1MB");
                    }
                }
                else
                {
                    return BadRequest("Unknown Format");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error: {ex} ");
            }
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var query = new GetTransactionsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

    }
}
