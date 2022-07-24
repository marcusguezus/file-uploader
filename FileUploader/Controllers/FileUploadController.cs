using FileUploader.DAL.Queries;
using FileUploader.Interface;
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
        // private FileValidation _settings;
        private readonly IMediator _mediator;
        private readonly IFileValidator _validator;

        public FileUploadController(IMediator mediator, IFileValidator validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileExtension = Path.GetExtension(file.FileName).TrimStart('.');

                if (_validator.ValidateFile(file.FileName, file.Length, out string message))
                {
                    var processor = ProcessorFactory.GetProcessorInstance(fileExtension, _mediator);

                    using (Stream stream = file.OpenReadStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string data = await reader.ReadToEndAsync();
                            var result = processor.ProcessFile(data);
                            if (!result.IsFileValid)
                            {
                                return BadRequest(result);
                            }
                            return Ok();
                        }
                    }
                }
                return BadRequest(message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error: {ex} ");
            }
        }

        [HttpGet("currency/{currency}")]
        public async Task<IActionResult> GetTransactionsByCurrency(string currency)
        {
            var query = new GetTransactionsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }



    }
}
