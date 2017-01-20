using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IFileRepository _fileRepository;
        private readonly IReleaseRepository _releaseRepository;

        private const string FileStoreDirectory = "/UploadedFiles/";

        public DownloadController(IFileRepository fileRepository, IReleaseRepository releaseRepository)
        {
            _fileRepository = fileRepository;
            _releaseRepository = releaseRepository;
        }

        [HttpGet]
        public FileResult DownloadFile(int Id)
        {
            if (Id == 0) return null;
            var file = _fileRepository.Get().FirstOrDefault(x => x.Id == Id);
            if (file == null) return null;
            string fileName = file.Name + "." + file.FileEnding;
            var fileBytes = GetBytesFromFile(file, 0);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

        [HttpGet]
        public FileResult DownloadRelease(int Id)
        {
            if (Id == 0) return null;
            var release = _releaseRepository.Get().FirstOrDefault(x => x.Id == Id);
            if (release == null) return null;
            if (release.RootFile == null) return null;
            var file = release.RootFile;
            string fileName = file.Name +"_Release"+ release.ReleaseNumber.ToString("X4")+"." + file.FileEnding;
            var fileBytes = GetBytesFromFile(file, release.ReleaseNumber);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private byte[] GetBytesFromFile(File file,int version)
        {
            if (file == null) return null;

            string category = file.Category.Name;
            string fileFolder = file.StorageName;

            string path = FileStoreDirectory + category + "\\" + fileFolder;
            string storefFileName = file.StorageName;
            if(version != 0)
            {
                storefFileName += "_";
                storefFileName += version.ToString("X4");
            }
            string fileName = storefFileName + "." + file.FileEnding;

            string downloadPath = Server.MapPath(path);
            string fullFilePath = downloadPath + "\\" + fileName;

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilePath);
            
            return fileBytes;
        }
    }
}