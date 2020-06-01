using Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.CommonHelpers
{
    public class FileHelpers
    {
        #region Upload Image

        public static void DeleteFile(string file)
        {
            var filepath = $"{AppHelpers.RESOURCE_PATH}/{file}";
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
        public static async Task<string> UploadFile(IFormFile file, string path = "")
        {
            if (file == null) return string.Empty;

            var dt = DateTime.Now;

            var filePath = string.Empty;

            if (string.IsNullOrEmpty(path))
            {
                path = "files";
            }
            string folderCreate = $"{path}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/";


            var currentdir = !string.IsNullOrEmpty(path) ? path : Directory.GetCurrentDirectory();
            var uploads = $"{AppHelpers.RESOURCE_PATH}/{folderCreate}"; // Path.Combine(AppConstants.RESOURCE_PATH ,folderCreate) ;

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var filename = StringHelper.UniqueKey(4) + file.FileName;
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            filePath = folderCreate + filename;

            return filePath;
        }

        public static async Task<List<string>> UploadMultiFile(ICollection<IFormFile> files, string path = "")
        {
            if (files == null || files.Count == 0) return new List<string>();
            var result = new List<string>();

            foreach (var file in files)
            {

                var image = await UploadFile(file, path);
                if (!string.IsNullOrEmpty(image))
                {
                    result.Add(image);
                }
            }
            return result;
        }
        #endregion

    }

}
