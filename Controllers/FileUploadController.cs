using File_Upload.Models;
using Microsoft.AspNetCore.Mvc;

namespace File_Upload.Controllers
{
    public class FileUploadController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(FileUpload fileUpload)
        {
            try
            {

                if (fileUpload == null || fileUpload.File == null || fileUpload.File.Length == 0)
                {
                    ViewBag.Message = "No file uploaded";
                    return View("Index");
                }
                if (fileUpload.File.Length > 2048)
                {
                    ViewBag.Message = "File should be less than 2 MB";
                    return View("Index");
                }
                    // validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(fileUpload.File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ViewBag.Message = "Only PNG, JPG, or JEPG files are allowed";
                    return View("Index");
                }

                var fileName = Guid.NewGuid().ToString() + fileExtension;


                // Define the path where the file will be saved
                var filePath = Path.Combine("Uploads", fileName);

          
                //Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileUpload.File.CopyToAsync(stream);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View("Index");

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error uploading file";
                Console.WriteLine(ex.ToString());
            }


            return View("Index");

        }
    }
}
