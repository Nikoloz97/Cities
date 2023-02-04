using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace Cities.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        // Allows for different types of files (doc, pdf, excel, etc.) to be supported
        // See program
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        // Create constructor that allows us to "dependency inject" FileExtensionContentTypeProvider into this controller
        public FilesController (FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
            { 
            // ?? = if anything left is null, then perform whatever's on right
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(nameof (fileExtensionContentTypeProvider));
        }



        [HttpGet("{fileId}")]
        public ActionResult GetFile(int fileId)
        {
            string pathToFile = "Gotsiridze_UWM_CoverLetter .docx";

            // Check if file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            // If we can't support the file type...
            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                // ... set it to default
                contentType = "application/octet-stream";
            }

            // Convert to bytes
            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            // make sure it's in the correct file type (doc, excel, pdf)
            // I.e. this tells operating system which "view" to use 
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
