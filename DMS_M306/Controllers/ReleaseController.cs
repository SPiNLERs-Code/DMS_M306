using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.Services;
using DMS_M306.ViewModels.File;
using DMS_M306.ViewModels.PhysicalStorage;
using DMS_M306.ViewModels.Release;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IFileRepository _fileRepository;
        private readonly IReleaseRepository _releaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;

        private const string FileStoreDirectory = "UploadedFiles";

        public ReleaseController(IFileRepository fileRepository, IReleaseRepository releaseRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _releaseRepository = releaseRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _fileService = FileService.GetInstance();
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null || Id == 0) return new HttpNotFoundResult();

            var query = _releaseRepository.Get(x => x.Id == Id);
            query = query.Include(x => x.LastModifiedBy);
            query = query.Include(x => x.ReleasedBy);
            query = query.Include(x => x.RootFile);
            var release = query.FirstOrDefault();
            if (release == null) return new HttpNotFoundResult();
            ReleaseDetailsViewModel vm = new ReleaseDetailsViewModel()
            {
                Description = release.Description,
                Id = release.Id,
                LastModifiedBy = release.LastModifiedBy.FullName,
                ReleaseDate = release.ReleaseDate,
                ReleasedBy = release.ReleasedBy.FullName,
                RootFileId = release.RootFileId,
                RootFileName = release.RootFile.Name,
                ReleaseNumber = release.ReleaseNumber,
                ReleaseDownloadPath = GetDownLoadPath(),
                PhysicalStorage = GetPhysicalStorageViewModel(release.RootFile)
            };
            return View(vm);
        }

        // GET: Release
        public ActionResult Index()
        {
            return RedirectToAction("File", "Index");
        }

        [HttpGet]
        public ActionResult CreateFor(int? Id)
        {
            if (Id == null || Id == 0) return new HttpNotFoundResult();
            var file = _fileRepository.Get(x => x.Id == Id).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();

            CreateForViewModel vm = new CreateForViewModel()
            {
                FileId = file.Id,
                FileName = file.Name
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateFor(CreateForViewModel vm)
        {
            
            var file = _fileRepository.Get(x => x.Id == vm.FileId).FirstOrDefault();
            if (file == null) return new HttpNotFoundResult();

            var allReleaseNumber = _releaseRepository.Get(x => x.RootFileId == vm.FileId).Select(x => x.ReleaseNumber).ToList();
            var releaseNumber = allReleaseNumber.Count == 0 ? 0 : allReleaseNumber.Max();
            releaseNumber++;
            Release newRelease = new Release
            {
                Description = vm.Description,
                LastModifiedBy = file.LastModifiedBy,
                ReleaseDate = DateTime.UtcNow,
                ReleasedBy = _userRepository.Get().FirstOrDefault(),
                ReleaseNumber = releaseNumber
            };

            //_releaseRepository.Add(newRelease);
            file.Releases.Add(newRelease);

            _fileRepository.Update(file);
            _unitOfWork.SaveChanges();
            if(file.StorageType != Enums.FileStorageType.PhysicalStorage)
            {
                string paht = GetFilePath(file);
                string originalFile = file.StorageName + "." + file.FileEnding;
                string releaseEnding = file.Releases.OrderBy(x => x.Id).LastOrDefault().Id.ToString("X8");
                string newName = file.StorageName + "_" + releaseEnding + "." + file.FileEnding;
                _fileService.CopyFile(paht, originalFile, newName);
            }
            return RedirectToAction("Details", "File", new { Id = file.Id });
        }

        private PhysicalStorageViewModel GetPhysicalStorageViewModel(File rootFile)
        {
            if (rootFile.PhysicalStorage == null) return null;
            return new PhysicalStorageViewModel
            {
                BuildingId = rootFile.PhysicalStorage.BuildingId,
                CabinetId = rootFile.PhysicalStorage.CabinetId,
                RoomId = rootFile.PhysicalStorage.RoomId,
                Id = rootFile.Id
            };
        }

        private string GetDownLoadPath()
        {
            return "";
        }

        private string GetFilePath(File file)
        {
            string categoryName = file.Category.Name;
            string storageName = file.StorageName;
            return GetFullFilePath(categoryName, storageName);
        }

        private string GetFullFilePath(string categoryName,string fileName)
        {
            var stringForPath = "\\"+ FileStoreDirectory+"\\" + categoryName + "\\" + fileName;
            return Server.MapPath(stringForPath);
        }
    }
}