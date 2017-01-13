using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using DMS_M306.ViewModels.Release;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReleaseController(IFileRepository fileRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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

            Release newRelease = new Release
            {
                Description = vm.Description,
                LastModifiedBy = file.LastModifiedBy,
                ReleaseDate = DateTime.UtcNow,
                ReleasedBy = _userRepository.Get().FirstOrDefault()
            };

            file.Releases.Add(newRelease);

            _fileRepository.Update(file);


            return RedirectToAction("Details", "File");
        }
    }
}