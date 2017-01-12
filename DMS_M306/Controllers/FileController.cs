using DMS_M306.Helpers;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
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
        private readonly IFileRepository _fileRepository;
        private readonly IFileCategoryRepository _fileCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const string FileStoreDirectory = "/UploadedFiles/";

        public FileController(IFileRepository fileRepository, IFileCategoryRepository fileCategoryRepository, IUserRepository userRepositor, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _fileCategoryRepository = fileCategoryRepository;
            _userRepository = userRepositor;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewFile()
        {
            var vm = new CreateFileViewModel()
            {
                Categories = GetCategories()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult NewFile(CreateFileViewModel createFileViewModel)
        {
            string storedName = "";
            if (!ModelState.IsValid)
            {
                return View(GetFullCreateViewModel(createFileViewModel, ""));
            }
            int uploadedFiles = 0;
            var category = _fileCategoryRepository.Get(x => x.Id == createFileViewModel.CategoryId).FirstOrDefault();
            string saveFolder = category.Name;
            string folderToStore = FileStoreDirectory + "/" + saveFolder;
            Directory.CreateDirectory(Server.MapPath(folderToStore));
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath(folderToStore);
                storedName = GetStoreName(Request.Files[upload].FileName, createFileViewModel.Name);
                string filename = storedName;
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
                uploadedFiles++;
            }
            if (uploadedFiles == 0)
            {
                return View(GetFullCreateViewModel(createFileViewModel, "Please select a valide file."));
            }
            CreateFile(createFileViewModel, storedName, category);
            return RedirectToAction("Index","Home");
        }

        private void CreateFile(CreateFileViewModel vm,string storeName, FileCategory category)
        {
            User currentUser = _userRepository.Get().FirstOrDefault();
            PhysicalStorage physicalStorage = null;
            Models.File file = new Models.File()
            {
                Category = category,
                Class = vm.Class,
                CreateDate = DateTime.UtcNow,
                CreatedBy = currentUser,
                Description = vm.Description,
                LastModified = DateTime.UtcNow,
                LastModifiedBy = currentUser,
                Name = vm.Name,
                Status = vm.Status,
                StorageName = storeName,
                StorageType = vm.StorageType,
                PhysicalStorage = physicalStorage
            };
            _fileRepository.Add(file);
            _unitOfWork.SaveChanges();
        }

        private CreateFileViewModel GetFullCreateViewModel(CreateFileViewModel vm, string ErrorMessage)
        {
            vm.Categories = GetCategories();
            vm.FormInformation = ErrorMessage;
            return vm;
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = _fileCategoryRepository.Get().ToList();
            var itemList = new List<SelectListItem>();
            foreach(var category in categories)
            {
                itemList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }
            return itemList;
        }

        private string GetStoreName(string oldName, string newName)
        {
            string dataEnding = oldName.Split('.').LastOrDefault();
            if (dataEnding == null) return null;
            string timeStamp = TimeHelper.GetTimestamp(DateTime.Now);
            string newFileName = newName + "_" + timeStamp + "." + dataEnding;
            return newFileName;
        }
    }
}