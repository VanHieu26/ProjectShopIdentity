namespace ProjectShopIdentity.Service
{
    public static class FileUpload
    {
        public static string UploadFile(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "default.png";
            }
            var path = Path.GetFullPath("./wwwroot/upload");
            var ext = new FileInfo(file.FileName).Extension.ToLower();
            var fileName = string.Format("{0}{1}", id, ext);

            var filePath = string.Format("{0}/{1}", path, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
    }
}
