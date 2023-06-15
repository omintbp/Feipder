namespace Feipder.Data.Repository
{
    public class ImageRepository
    {
        public async Task<string> UploadImage(string folderName, IFormFile file, IWebHostEnvironment env)
        {
            try
            {
                var path = env.WebRootPath + $"/{folderName}/";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); ;

                path += $"{fileName}";
                
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return $"{folderName}/{fileName}";

            } catch(Exception ex)
            {
                throw;
            }
        }
    }
}
