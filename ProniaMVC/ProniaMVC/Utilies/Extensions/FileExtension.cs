namespace ProniaMVC.Utilies.Extensions
{
    public static class FileExtension
    {
        public static bool CheckType(this IFormFile file, string type) 
            => file.ContentType.Contains(type);
        public static bool CheckSize(this IFormFile file, int kb)
            => kb * 1024 > file.Length;
        public static string SaveFile(this IFormFile file, string path)
        {
            string fileName = ChangeFileName(file.FileName);
            using (FileStream filestream = new FileStream(Path.Combine(path,fileName),FileMode.Create))
            {
                file.CopyTo(filestream);
            }
            return fileName;
        }

        static string ChangeFileName(string oldName)
        {
            string extension = oldName.Substring(oldName.LastIndexOf("."));
            if (oldName.Length<32)
            {
                oldName = oldName.Substring(0, oldName.LastIndexOf("."));
            }
            else
            {
                oldName = oldName.Substring(0, 31);
            }
            string newName = Guid.NewGuid() + oldName + extension;
            return newName;
        }

        public static void DeleteFile(this string root, string folder, string image)
        {
            string filepath = Path.Combine(root, folder,image);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
