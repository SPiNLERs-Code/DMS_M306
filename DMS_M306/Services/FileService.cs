using DMS_M306.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace DMS_M306.Services
{
    public class FileService : IFileService
    {
        private static FileService _instance = new FileService();

        private FileService()
        {

        }

        public static FileService GetInstance()
        {
            return _instance;
        }

        public void SaveFile(HttpPostedFileWrapper file, string pathToSave, string filename)
        {
            Directory.CreateDirectory(pathToSave);
            file.SaveAs(Path.Combine(pathToSave, filename));
        }

        public void CopyFile(string storedPath, string oldFileName, string newFileName)
        {
            string originalFile = storedPath + "\\" + oldFileName;
            string newFile = storedPath + "\\" + newFileName;
           
            
            File.Copy(originalFile, newFile);
        }
    }
}