using Microsoft.AspNetCore.Mvc;

namespace Cities.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        [HttpGet("{fileId}")]
        public ActionResult GetFile(int fileId)
        {
            string pathToFile = "Gotsiridze_UWM_CoverLetter .docx";

            // Check if file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            // Convert to bytes
            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            return File(bytes, "text/plain", Path.GetFileName(pathToFile));
        }
    }
}
