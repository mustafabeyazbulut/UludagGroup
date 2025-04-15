namespace UludagGroup.Commons
{
    public class ImageOperations
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public string FilePath = "Photos";
        public ImageOperations(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public Task<string> UploadImageAsync(IFormFile ImageFile)
        {
            string uniqueFileName = Guid.NewGuid().ToString("N");
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, FilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileExtension = Path.GetExtension(ImageFile.FileName);
            uniqueFileName = $"{uniqueFileName}{fileExtension}";
            string filePath;

            if (string.IsNullOrEmpty(uniqueFileName))
            {
                filePath = Path.Combine(folderPath, " ");
            }
            else
            {
                filePath = Path.Combine(folderPath, uniqueFileName);
            }

            // Dosyayı bu yola kaydedin
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ImageFile.CopyTo(fileStream);
            }
            return Task.FromResult(uniqueFileName);
        }
        public Task<bool> DeleteIconAsync(string FileName)
        {
            if (FileName == null) return Task.FromResult(false);
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, FilePath);

            string filePath;
            if (string.IsNullOrEmpty(FileName))
            {
                filePath = Path.Combine(folderPath, " ");
            }
            else
            {
                filePath = Path.Combine(folderPath, FileName);
            }


            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return Task.FromResult(true);
        }
    }
}
