using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WebServices.Code.Helpers
{
    public interface IFileHelper
    {
        string GetImageUrl(string path);
        string MoveTempFile(string sourceFile, string targetPath);
        Task<string> UploadTempFile(IFormFile formFile, int sizetype = 0);

    }
    public class FileHelper : IFileHelper
    {
        private readonly SharedOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileHelper(IOptions<SharedOptions> optionsAccessor, IHostingEnvironment hostingEnvironment)
        {
            _options = optionsAccessor.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        private bool IsTempPath(string path)
        {
            return !string.IsNullOrEmpty(path) && path.Contains(_options.ResourceTempDir);
        }
        public string MoveTempFile(string sourceFile, string targetPath)
        {
            if (!IsTempPath(sourceFile))
            {
                return sourceFile;
            }
            sourceFile = Path.Combine(_options.ResourcePath, sourceFile);
            if (File.Exists(sourceFile))
            {
                string newPath = $"{targetPath}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";

                var destinationPath = Path.Combine(_options.ResourcePath, newPath);
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                var fileName = Path.GetFileName(sourceFile);
                var destinationFile = Path.Combine(destinationPath, fileName);
                File.Move(sourceFile, destinationFile);
                return $"{newPath}/{fileName}";
            }
            return string.Empty;
        }
        public string GetImageUrl(string path)
        {
            return $"{_options.ResourceServer}/{path}";
        }

        public async Task<string> UploadTempFile(IFormFile formFile, int sizetype = 0)
        {
            if (formFile == null || formFile.Length == 0) return string.Empty;

            var fileext = Path.GetExtension(formFile.FileName);
            var filename = $"{StringHelper.UniqueKey(10)}{fileext}";

            var targetPath = Path.Combine(_options.ResourcePath, _options.ResourceTempDir);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            if (sizetype == 0)
            {
                using (var stream = new FileStream(Path.Combine(targetPath, filename), FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

            }
            else
            {
             

                using (Image image = Image.Load(formFile.OpenReadStream()))
                {
                    const int size = 640;
                    /*
                    int width , height ;
                    if (image.Width > image.Height && image.Width > size)
                    {
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else if (image.Height > size)
                    {
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    else
                    {
                        width = image.Width;
                        height = image.Height;
                    }
                    */

                    image.Mutate(x => x
                         .Resize(new ResizeOptions
                         {
                             Size = new SixLabors.Primitives.Size(size),
                             Mode = ResizeMode.Crop
                         }));

                    image.Save(Path.Combine(targetPath, filename)); // Automatic encoder selected based on extension.
                }


            }
            return $"{ _options.ResourceTempDir}/{filename}";
        }



        #region Upload Image
        /*
        public static void DeleteFile(string file)
        {
            var filepath = $"{AppConstants.RESOURCE_PATH}/{file}";
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
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

        */
        #endregion
    }
}
