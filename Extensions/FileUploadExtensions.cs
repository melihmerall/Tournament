namespace Tournament.Extensions
{
    public static class FileUploadExtensions
    {
        public static async Task<string> UploadFile(this IFormFile file, string uploadPath)
        {
            if (file != null && file.Length > 0)
            {
                // Dosya adını alma
                var fileName = Path.GetFileName(file.FileName);

                // Dosyayı belirli bir dizine kaydetme
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filePath; // Dosyanın tam yolu ile geri dönme
            }

            return null; // Dosya seçilmediyse veya bir hata oluştuysa null döndürme
        }
    }
}