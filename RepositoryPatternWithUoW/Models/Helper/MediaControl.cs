using Microsoft.AspNetCore.Http;
using RepositoryPatternWithUoW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Helper
{
    public static class MediaControl
    {
        public static async Task<string> Upload(FilePath filePath, IFormFile file)
        {
            var folderName = "";
            switch (filePath)
            {
                case FilePath.EmployeeImage:
                    folderName = Path.Combine("wwwroot", "Images", "EmployeeImage");
                    break;
            }
            string extension = Path.GetExtension(file.FileName);
            var UinqueName = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            using (var filestream = new FileStream(Path.Combine(folderName, UinqueName + extension), FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }
            return UinqueName + extension;
        }
        public static string GetPath(FilePath filePath)
        {
            switch (filePath)
            {
                case FilePath.EmployeeImage:
                    return "/Images/EmployeeImage/";
                default:
                    return null;
            }
        }
    }
}
