using Microsoft.AspNetCore.StaticFiles;
namespace Com.Gosol.KNTC.API.Controllers.Common
{


    public class Common
    {
        public Common()
        {
        }
        public bool UploadFiles(IFormFile files, int NguoiCapNhat, string fileName)
        {
            try
            {
                //var file = Request.Form.Files[0];
                var folderName = Path.Combine("Upload", "FileDinhKemDuyetKeKhai");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        files.CopyTo(stream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

      
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public string ConvertFileToBase64(string pathFile)
        {
            try
            {
                var at = System.IO.File.GetAttributes(pathFile);

                byte[] fileBit = System.IO.File.ReadAllBytes(pathFile);
                var file = System.IO.Path.Combine(pathFile);

                string AsBase64String = Convert.ToBase64String(fileBit);
                return AsBase64String;
            }
            catch (Exception ex)
            {
                return string.Empty;
                throw ex;
            }
        }

    }



}