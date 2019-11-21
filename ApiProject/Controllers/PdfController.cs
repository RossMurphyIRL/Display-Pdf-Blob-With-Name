using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiProject.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        

        private readonly ILogger<PdfController> _logger;
        private IHostEnvironment _hostingEnvironment;


        public PdfController(ILogger<PdfController> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _hostingEnvironment = environment;
        }

        [HttpGet]
        [Route("PreviewPdf")]
        [ActionName("PreviewPdf")]
        public IActionResult PreviewDocx()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "sample.Pdf");
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            var fileResult = new Response
            {
                ContentType = "application/pdf",
                FileName = "This is the new name of the file.pdf",
                Document = ms
            };
            var response = File(fileResult.Document, fileResult.ContentType);
            response.FileDownloadName = fileResult.FileName;
            Response.Headers.Add("FileDownloadName", response.FileDownloadName);
            Response.Headers.Add("Access-Control-Expose-Headers", "filedownloadname");
            return response;
        }
    }
    public class Response
    {
        public MemoryStream Document { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }

}
