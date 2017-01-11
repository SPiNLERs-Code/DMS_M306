using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.ViewModels.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    public class FileController : Controller
    {
        private IFileRepository _fileRepository;
        private IUnitOfWork _unitOfWork;

        public FileController(IFileRepository fileRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(FileViewModel fileViewModel)
        {
            if (!ModelState.IsValid) return View();
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/UploadedFiles/");
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                //string filename = fileViewModel.Name;
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }
            return View(fileViewModel);
        }
    }
}