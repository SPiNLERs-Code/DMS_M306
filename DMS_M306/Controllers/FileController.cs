using DMS_M306.Constants;
using DMS_M306.Enums;
using DMS_M306.Helpers;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.Services;
using DMS_M306.ViewModels.Change;
using DMS_M306.ViewModels.File;
using DMS_M306.ViewModels.PhysicalStorage;
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
        private FileService _fileService;

        public FileController(IFileRepository fileRepository, IFileCategoryRepository fileCategoryRepository, IUserRepository userRepositor, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _fileCategoryRepository = fileCategoryRepository;
            _userRepository = userRepositor;
            _unitOfWork = unitOfWork;
            _fileService = FileService.GetInstance();
        }

        [HttpGet]
        public ActionResult Index()
        {
            FilesViewModel vm = new FilesViewModel();
            vm.Files = GetTopFiles(20);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0) return new HttpNotFoundResult();
            var file = _fileRepository.Get(x => x.Id == Id).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();
            EditFileViewModel vm = new EditFileViewModel()
            {
                CategoryName = file.Category.Name,
                Class = file.Class,
                Description = file.Description,
                Name = file.Name,
                PhysicalStorageBuildingId = file.PhysicalStorage != null ? file.PhysicalStorage.BuildingId : "",
                PhysicalStorageCabinetId = file.PhysicalStorage != null ? file.PhysicalStorage.CabinetId : "",
                PhysicalStorageRoomId = file.PhysicalStorage != null ? file.PhysicalStorage.RoomId : "",
                Status = file.Status,
                StorageType = file.StorageType
            };
            vm = GetFullEditViewModel(vm, file, "");
            return View(vm);
        }

        [HttpGet]
        public ActionResult Details(int? Id)
        {
            if (Id == null || Id == 0) return new HttpNotFoundResult();
            var file = _fileRepository.Get(x => x.Id == Id).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();

            FileDetailsViewModel vm = GetFileDetails(file);

            return View(vm);
        }
        [HttpPost]
        public ActionResult Edit(EditFileViewModel vm)
        {
            bool isContentChanged = false;
            bool isFileChanged = false;
            if (vm.Id == 0) return new HttpNotFoundResult();
            var file = _fileRepository.Get(x => x.Id == vm.Id).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();
            if (!ModelState.IsValid) return View(GetFullEditViewModel(vm, file, ""));


            isContentChanged = IsContentChanged(file, vm);

            DateTime dateNow = DateTime.UtcNow;
            Models.User currentUser = _userRepository.Get().FirstOrDefault();

            if (isContentChanged)
            {
                file.Class = vm.Class;
                file.Description = vm.Description;
                file.LastModified = dateNow;
                file.Status = vm.Status;
                if (file.PhysicalStorage != null)
                {
                    file.PhysicalStorage.BuildingId = vm.PhysicalStorageBuildingId;
                    file.PhysicalStorage.CabinetId = vm.PhysicalStorageCabinetId;
                    file.PhysicalStorage.RoomId = vm.PhysicalStorageRoomId;
                }
            }
            if (file.StorageType != FileStorageType.PhysicalStorage)
            {
                var uploadState = GetFileChangeState(file, Request);

                if (uploadState == FileUploadStates.WrongDateType)
                {
                    vm.FormInformation = "Wrong data type.";
                    return View(vm);
                }

                if (uploadState == FileUploadStates.Success)
                {
                    isFileChanged = true;
                    file.LastModified = dateNow;
                }
            }

            if (!isFileChanged && !isContentChanged)
            {
                TempData[GlobalConstants.AlertInfoMessageKey] = GlobalConstants.FileEditNoChanges;
                return RedirectToAction("Details", "File", new { @Id = file.Id });
            }
            

            Change change = new Change()
            {
                ChangeDate = dateNow,
                ChangedBy = currentUser,
                Description = GetChangeDescription(isFileChanged, isContentChanged)
            };

            try
            {
                file.Changes.Add(change);
                _fileRepository.Update(file);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                TempData[GlobalConstants.AlertErrorMessageKey] = GlobalConstants.FileEditError;
                return RedirectToAction("Index");
            }
            
            TempData[GlobalConstants.AlertSuccessMessageKey] = GlobalConstants.FileEditSuccess;
            return RedirectToAction("Details", "File", new { @Id = file.Id });
        }

        [HttpGet]
        public ActionResult Create()
        {
            if(_fileCategoryRepository.Get().Count() == 0)
            {
                this.TempData[GlobalConstants.AlertInfoMessageKey] = GlobalConstants.CreateCategoryFirstMessage;
                return RedirectToAction("Create", "Category");
            }
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

            if (createFileViewModel.IsFilePhysical)
            {
                CreateFile(createFileViewModel, fileAndFolderName, category);
            }
            else
            {
                string ending = SaveFileReturnDataEnding(fileAndFolderName, folderToStore, Request);

                if (String.IsNullOrWhiteSpace(ending))
                {
                    return View(GetFullCreateViewModel(createFileViewModel, "Please select a valide file."));
                }
                CreateFile(createFileViewModel, fileAndFolderName, category, ending);
            }

            TempData[GlobalConstants.AlertSuccessMessageKey] = GlobalConstants.FileCreateSuccess;
            return RedirectToAction("Index", "Home");
        }

        private string SaveFileReturnDataEnding(string newName, string folder, HttpRequestBase request)
        {
            string dataEnding = "";
            foreach (string upload in request.Files)
            {
                HttpPostedFileWrapper requesFile = (HttpPostedFileWrapper)request.Files[upload];
                if (requesFile.ContentLength == 0) continue;
                string pathToSave = Server.MapPath(folder);
                dataEnding = requesFile.FileName.Split('.').LastOrDefault();

                if (String.IsNullOrWhiteSpace(dataEnding)) return "";
                string filename = newName + "." + dataEnding.ToLower();
                _fileService.SaveFile(requesFile, pathToSave, filename);
            }
            return dataEnding;
        }

        private void CreateFile(CreateFileViewModel vm, string storeName, FileCategory category, string dataEnding = "")
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

        private EditFileViewModel GetFullEditViewModel(EditFileViewModel vm, Models.File file, string ErrorMessage)
        {
            vm.CategoryName = file.Category.Name;
            vm.Name = file.Name;
            vm.FormInformation = ErrorMessage;
            vm.StorageType = file.StorageType;
            vm.FileId = file.Id;
            vm.FileType = file.FileEnding;
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
                Releases = GetReleases(file.Releases),
                Changes = GetChanges(file.Changes),
                PhysicalStorage = GetPhysicalStorage(file.PhysicalStorage)
            };

            return vm;
        }

        private PhysicalStorageViewModel GetPhysicalStorage(PhysicalStorage storage)
        {
            if (storage == null) return null;

            return new PhysicalStorageViewModel()
            {
                BuildingId = storage.BuildingId,
                CabinetId = storage.CabinetId,
                Id = storage.Id,
                RoomId = storage.RoomId
            };
        }

        private string GetDownLoadPath(FileStorageType type, string filename, string fileEnding, FileCategory category)
        {
            if (type == FileStorageType.PhysicalStorage) return null;
            return "";
        }

        private List<ReleaseViewModel> GetReleases(List<FileRelease> releases)
        {
            List<ReleaseViewModel> allReleases = new List<ReleaseViewModel>();

            foreach (var item in releases)
            {
                var newReleaseViewModel = new ReleaseViewModel
                {
                    Description = item.Description,
                    Id = item.Id,
                    LastModifiedBy = item.LastModifiedBy.FullName,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedBy = item.ReleasedBy.FullName,
                    ReleaseNumber = item.ReleaseNumber,
                    IsActive = item.IsActive
                };
                allReleases.Add(newReleaseViewModel);
            }
            allReleases = allReleases.OrderByDescending(x => x.ReleaseNumber).ToList();
            return allReleases;
        }

        private List<ChangeViewModel> GetChanges(List<Change> changes)
        {
            List<ChangeViewModel> allReleases = new List<ChangeViewModel>();

            foreach (var item in changes)
            {
                var newReleaseViewModel = new ChangeViewModel
                {
                    Description = item.Description,
                    ChangeDate = item.ChangeDate,
                    ChangedBy = item.ChangedBy.FullName,
                    FileId = item.Id
                };
                allReleases.Add(newReleaseViewModel);
            }
            allReleases = allReleases.OrderByDescending(x => x.ChangeDate).ToList();
            return allReleases;
        }

        private bool IsContentChanged(Models.File file, EditFileViewModel createViewModel)
        {
            if (file.Class != createViewModel.Class) return true;
            if (file.Description != createViewModel.Description) return true;
            if (file.Status != createViewModel.Status) return true;

            if(file.PhysicalStorage != null)
            {
                if (file.PhysicalStorage.BuildingId != createViewModel.PhysicalStorageBuildingId) return true;
                if (file.PhysicalStorage.CabinetId != createViewModel.PhysicalStorageCabinetId) return true;
                if (file.PhysicalStorage.RoomId != createViewModel.PhysicalStorageRoomId) return true;
            }
            return false;
        }
        private FileUploadStates GetFileChangeState(Models.File file, HttpRequestBase request)
        {
            int count = 0;
            foreach (string upload in request.Files)
            {
                HttpPostedFileWrapper requesFile = (HttpPostedFileWrapper)request.Files[upload];
                if (requesFile.ContentLength == 0) continue;
                var folder = FileStoreDirectory + file.Category.Name + "\\" + file.StorageName;
                string pathToSave = Server.MapPath(folder);
                var dataEnding = requesFile.FileName.Split('.').LastOrDefault();

                if (String.IsNullOrWhiteSpace(dataEnding)) return FileUploadStates.WrongDateType;
                if (dataEnding.ToLower() != file.FileEnding.ToLower()) return FileUploadStates.WrongDateType;

                string filename = file.StorageName+ "." + dataEnding.ToLower();
                _fileService.SaveFile(requesFile, pathToSave, filename);
                count++;
            }
            if (count < 1)
            {
                return FileUploadStates.NoFile;
            }
            return FileUploadStates.Success;
        }

        private string GetChangeDescription(bool hasFileChanged, bool hasInfosChanged)
        {
            if (hasFileChanged && hasInfosChanged) return "Infos and file changed";
            if (hasFileChanged) return "File changed";
            if (hasInfosChanged) return "Infos changed";
            return "Nothing changed";
        }
    }
}