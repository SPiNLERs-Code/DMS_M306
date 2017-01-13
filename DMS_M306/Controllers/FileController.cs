using DMS_M306.Helpers;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            FilesViewModel vm = new FilesViewModel();
            vm.Files = GetTopFiles(20);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            if(Id == null||Id == 0) return new HttpNotFoundResult();
            var file = _fileRepository.Get(x => x.Id == Id).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();

            FileDetailsViewModel vm = GetFileDetails(file);

            return View(vm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var vm = new CreateFileViewModel()
            {
                Categories = GetCategories()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CreateFileViewModel createFileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(GetFullCreateViewModel(createFileViewModel, ""));
            }

            string timeStamp = TimeHelper.GetTimestamp(DateTime.Now);
            string fileAndFolderName = createFileViewModel.Name.ToLower() + "_" + timeStamp;
            FileCategory category = _fileCategoryRepository.Get(x => x.Id == createFileViewModel.CategoryId).FirstOrDefault();

            string folderToStore = FileStoreDirectory + "/" + category.Name + "/" + fileAndFolderName;

            string ending = SaveFileReturnDataEnding(fileAndFolderName, folderToStore, Request);

            if (String.IsNullOrWhiteSpace(ending))
            {
                return View(GetFullCreateViewModel(createFileViewModel, "Please select a valide file."));
            }
            CreateFile(createFileViewModel, fileAndFolderName, ending, category);
            return RedirectToAction("Index", "Home");
        }

        private string SaveFileReturnDataEnding(string newName, string folder, HttpRequestBase request)
        {
            string dataEnding = "";
            foreach (string upload in request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath(folder);
                dataEnding = request.Files[upload].FileName.Split('.').LastOrDefault();
                if (String.IsNullOrWhiteSpace(dataEnding)) return "";
                string filename = newName + "." + dataEnding.ToLower();
                Directory.CreateDirectory(Server.MapPath(folder));
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }
            return dataEnding;
        }

        private void CreateFile(CreateFileViewModel vm, string storeName, string dataEnding, FileCategory category)
        {
            User currentUser = _userRepository.Get().FirstOrDefault();
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
                PhysicalStorage = CreatePhysicalStorageWhenRequired(vm),
                FileEnding = dataEnding.ToLower()
            };
            _fileRepository.Add(file);
            _unitOfWork.SaveChanges();
        }

        private PhysicalStorage CreatePhysicalStorageWhenRequired(CreateFileViewModel vm)
        {
            if (!vm.IsFilePhysical) return null;
            return new PhysicalStorage
            {
                BuildingId = vm.PhysicalStorageBuildingId,
                CabinetId = vm.PhysicalStorageCabinetId,
                RoomId = vm.PhysicalStorageRoomId
            };
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
            foreach (var category in categories)
            {
                itemList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }
            return itemList;
        }

        private string GetStoreName(string oldName, string newName, string timestamp, bool hasEnding = true)
        {
            string dataEnding = oldName.Split('.').LastOrDefault();
            if (dataEnding == null) return null;
            string newFileName = newName + "_" + timestamp;
            if (hasEnding)
            {
                newFileName += "." + dataEnding;
            }
            return newFileName;
        }

        private List<FileViewModel> GetTopFiles(int count)
        {
            var query = _fileRepository.Get();
            query = query.Include(x => x.Category);
            query = query.Include(x => x.Changes);
            query = query.Include(x => x.CreatedBy);
            query = query.Include(x => x.LastModifiedBy);
            query = query.Include(x => x.PhysicalStorage);
            query = query.Include(x => x.Releases);
            query = query.OrderBy(x => x.LastModified).Take(count);
            var file = query.ToList();

            List<FileViewModel> fileList = file.Select(x => new FileViewModel
            {
                Category = x.Category.Name,
                Class = x.Class,
                CreateDate = x.CreateDate,
                CreatedBy = x.CreatedBy.FullName,
                Description = x.Description,
                Id = x.Id,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy.FullName,
                Name = x.Name,
                ReleaseCount = x.Releases.Count(),
                Status = x.Status,
                StorageType = x.StorageType
            }).ToList();

            return fileList;
        }

        private FileDetailsViewModel GetFileDetails(Models.File file)
        {
            FileDetailsViewModel vm = new FileDetailsViewModel
            {
                Category = file.Category.Name,
                Class = file.Class,
                CreateDate = file.CreateDate,
                CreatedBy = file.CreatedBy.FullName,
                Description = file.Description,
                Id = file.Id,
                LastModified = file.LastModified,
                LastModifiedBy = file.LastModifiedBy.FullName,
                Name = file.Name,
                ReleaseCount = file.Releases != null ? file.Releases.Count : 0,
                Status = file.Status,
                StorageType = file.StorageType,
                Releases = GetReleases(file.Releases)
            };

            return vm;
        }

        private List<ReleaseViewModel> GetReleases(List<Release> releases)
        {
            List<ReleaseViewModel> allReleases = new List<ReleaseViewModel>();

            foreach(var item in releases)
            {
                var newReleaseViewModel = new ReleaseViewModel
                {
                    Description = item.Description,
                    Id = item.Id,
                    LastModifiedBy = item.LastModifiedBy.FullName,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedBy = item.ReleasedBy.FullName
                };
                allReleases.Add(newReleaseViewModel);
            }

            return null;
        }
    }
}